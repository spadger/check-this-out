# check-this-out
Checkout.com Technical test by Jon Bates

I attest that this solution is a clean-room implementation of the checkout.com [technical-test](Checkout-dotnet-Challenge-2.0.pdf) and all work is my own.

## Prerequisites
This application was developed in Windows using dotNet Core 3.1, WSL & Linux containers.  It has not been tested on OSX or any Linux distribution

## Running

There are two main ways to run:
* Directly from Visual Studio, which should launch https://localhost:5001/swagger
* `docker-compose up --build` from the solution root.
  * http://localhost/swagger & http://localhost:9090
  * This [link](http://localhost:9090/graph?g0.range_input=1m&g0.expr=payment_request_success_count%20%2F%20payment_request_total_count&g0.tab=0]) will take you to the ratio of successful vs unsuccessful payments.

Any transaction you submit can be retrieved
* Use the following CVVs to simulate some acquiring banks errors:
  * 500 - General Exception
  * 400 - Validation failure (i.e. request DTO was valid, but the request was rejected by the acquiring bank)

## CI
CI was implemented as a simple github action - https://github.com/spadger/check-this-out/actions.  Currently it just restores and builds, setting the project's status upon completion.

## Assumptions
* There is only one acquiring bank whose task it is to interact with the cardholder's actual bank whichever that may be.  i.e. this payment gateway isn't connected to BACS or the Faster Payments system, but exists simply to provide a stable interface to wider banking infrastructure.
* Calls to the banking infrastructure are synchronous, since real-world card-processing SLAs are ~200ms - the application will wait for a response and record it.
  * Submitting a transaction to an asynchronous API would have returned HTTP 202 (Accepted) with a link `GET /{paymentRequestId}` identifying where to poll for a result
* In a real-world scenario, we'd lock down or disable the swagger-ui endpoint
* A very basic idempotency mechanism exists - If a payment is submitted twice with the same request id, the client will receive an HTTP 419 (conflict) response

## Extra-mile bonus things I left out

* Authentication - Obviously a prod API would need Authentication & Authorization, but configuring JWT infrastructure feels out of scope
* API Client - SwaggerUI os a reasonable client for this
* Performance testing - feels out of scope
* Storage & Encryption - All storage is mocked for this exercise, so no encryption / tokenisation is required

## Additional things I considered, but were out of scope 
* Structured logging.  Its really useful generally, but overkill for this tech-demo.  We'll log plain messages to the console for brevity
* OpenTacing. Again, something I'd recommend in a distributed environment, but not entirely useful with only one application
* Lots of application metadata associated with requests & responses, e.g. client version, application version, machine id, correlation, causation & request Ids
* I would have liked to use the IETF Problem specification, https://tools.ietf.org/html/rfc7807, but it gives diminishing returns
* Grafana dashboards

## Other Notes
When architecting a high-level solution, I prefer to split by module (i.e. broad business responsibility) rather than by layer (data-layer, ports, adapters etc)
