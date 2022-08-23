# Retry Policy Presentation

Welcome to the test project. You've no doubt come from a meetup or something. Please feel free to check this code out and play with it. I'm not actively seeking contributions for this as it's a test project meant for presentations.

![qrcode](./docs/presentation.png)

Sharable link

## What is this?

The internet is a very wild and wacky place, we must all consider how to best meet our consumer's expectations. What happens when you have breaks in this hyper connected environment? Should you fail fast or retry? Or have fallback options?

- Standard retry policy
- Exponential retry policy
- Circuit breaker

### Things to consider

- Fallback values? Is your business comfortable with serving failures
- Alternatives to use? EG, agnostic clients like payment service providers
- Read-Only alternatives / failover
- Does your service blow out other people's SLOs?
- Should the right course of action be that your service fails.

## How to run this

Open the [Soltuion](./Policy.sln) and the project should be configured to run the [Server](./Server/) without debugging and the [WantACracker](./WantACracker/) project with debugging - Some ports may need fiddling sorry.

This project uses [Polly](https://github.com/App-vNext/Polly) as the foundation of this. It's a test-bed for experimentations and is not exhaustive.

### Requirements

- c# dotnet 5.0
- browser
- (optional) IDE

## Notes from the author

I'm sorry the naming conventions are a bit rough. This is only meant to be a quick demo.
