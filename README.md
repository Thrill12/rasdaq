# Rasdaq

A simple, 2D game engine written in C#.

rasdaq uses a simple Entity Component System Architecture (ECS). It contains:
- Input management
- Basic 2D Sprite renderer
- Logging

## Documentation

You can find our documentation here:
https://thrill12.github.io/rasdaq/docs/introduction.html

To run a local documentation site for your branch, please:
- install docfx using `dotnet tool update -g docfx`
- run `docfx docfx.json --serve` from the root directory of the repo
- access the site using `localhost:8080`

## Tests

To run tests, execute the command `dotnet test` from the root directory

## Samples

Samples serve as example projects of how you can use rasdaq. These try to offer best practices when using rasdaq.

To run a sample, execute the command `dotnet run --project samples/<SAMPLE_NAME>`. 
For example:
`dotnet run --project samples/pong` will run the 'Pong' sample.

## Contributions

Currently we will not accept contributions as it is a team project, but this may open up in the future.
