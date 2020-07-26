# check-this-out
Checkout.com Technical test by Jon Bates

I attest that this solution is a clean-room implementation of the checkout.com [technical-test](Checkout-dotnet-Challenge-2.0.pdf) and all work is my own.

## Prerequisites
This application was developed in Windows using dotNet Core 3.1, WSL & linux containers.  It has not been tested on OSX or any Linux distribution

## Running

You can excercise the API from https://localhost/swagger

## Assumptions
* There is only one acquiring bank whose task it is to interact with the cardholder's actual bank whichever that may be.  i.e. this payment gateway isn't connected to BACS or the Faster Payments system, but exists simply to provide a stable interface to wider banking infrastructure.

## Things I left out

* Structured logging.  Its really useful generally, but overkill for this tech-demo.  We'll log plain messages to the console for brevity
* OpenTacing. Again, something I'd recommend in a distributed environment, but not entirely useful with only one application

## Other Notes
When architecting a high-level solution, I prefer to split by module (i.e. broad business responsibility) rather than by layer (data-layer, ports, adapters etc)
I am not particularly happy with the current domain model for 2 reasons:
 1. The namespacing isn't great since both the bank module and the payment-store module use the same representations, meaning those classes end up in a lombo `Domain` namespace
 2. I am used to Kotlin's sealed classes (the closest dotnet equivalent is F#'s [discriminated union](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/discriminated-unions)).  I'd have loved the domain's response types to have been closer to that but I didn't want to go down some domain-modelling rabbit hole when the entire excercise is fairly wide-ranging