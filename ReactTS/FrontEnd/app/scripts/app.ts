console.log('Hello, app.ts!');

// here we reuse server model in TypeScript automatically without 
// copying or references
var a: server.SampleModel =
    {
        id: 1,
        value: "asd",
        dateTime: new Date(2014, 6, 18)
    };