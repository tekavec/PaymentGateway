API_IMAGE_NAME := payment-gateway-api
VERSION := $(shell versioner VERSION 2>/dev/null || echo `cat VERSION`-dev)
BUILD_IMAGE := mcr.microsoft.com/dotnet/core/sdk:2.2-stretch
BUILD_PATH := /opt/workspace/tmp/build
NUGET_SOURCES := --source https://api.nuget.org/v3/index.json

.PHONY: build clean clean-containers clean-temp-files run-acquirer run-tests test version

define build_run
	docker run --rm -v $(CURDIR)://opt/workspace -v /~/.nuget/packages://root/tmp/packages -w //opt/workspace $(BUILD_IMAGE) bash -c "$(1)"
endef

build: compile
	@echo "*** Building image version: "$(VERSION)
	@test -s ./tmp/build/PaymentGateway.dll || { echo "Cannot find API library. Please run 'make compile' before attempting to build image. Exiting..."; exit 1; }
	docker build --build-arg BUILD_VERSION=v$(VERSION) -t $(API_IMAGE_NAME) .

clean: clean-containers clean-temp-files

clean-containers:
	docker-compose down -v --remove-orphans

clean-temp-files:
	$(call build_run,find . -type d \( -name bin -o -name obj -o -name TestResults -o -name tmp \) -exec rm -rf '{}' +;)

compile:
	@echo "*** Compiling projects ***"
	$(call build_run,dotnet restore $(NUGET_SOURCES) --packages /root/tmp/packages && dotnet clean -c Release && dotnet publish --no-restore -c Release -o $(BUILD_PATH) /property:Version=$(VERSION))

run-acquirer:
	docker-compose down -v --remove-orphans
	docker-compose up --force-recreate -d

run-tests:
	docker-compose -f docker-compose.test.yml up --force-recreate --abort-on-container-exit 
# enumerate all tests and print the failure count
	@for x in test/**/TestResults/*.trx; do \
		result=$$(sed -n 's/.*failed="\([^"]*\).*/\1/p' $$x) \
		&& echo Test results for $$x: $$result; \
	done
# fail build if failure count != 0
	@for x in test/**/TestResults/*.trx; do \
		result=$$(sed -n 's/.*failed="\([^"]*\).*/\1/p' $$x) \
		&& if [ $$result -ne 0 ] ; then echo "At least one failing test detected, aborting!" && exit 1 ; fi \
	done
# all green
	@echo All tests have passed!!!

test: clean compile run-tests clean-containers

version:
	@echo $(VERSION)