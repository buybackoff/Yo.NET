Yo.NET
===========
A template application showing how to integrate ServiceStack and SignalR on the server side, and how to develop the 
client side independently from the server. Only server API is used by clients.

TypeScript is used to define server API. With Web Essentials 2.1 TypeScript definition files could be 
generated automatically from C# models. Gulp's `servertypes` task updates all server definitions
for the front end app.

Front end development is done with Yeoman, Bower, Gulp and related tools. Both back end and front end
are hosted in an initially empty ASP.NET application (not MVC, not WebForms). All public contracts
that could be used by clients are defined in `Contracts` project, and all services and hubs are 
implemented in `ServiceImplementations` project.

More things to come, yo!

Preparation
-----------
The very initials steps to start the app.

* Prepare your environment by installing Node.js, npm, git, grunt, bower, gulp, [yeoman](http://yeoman.io/codelab/setup.html). 
Setup TypeScript compilation on save (VS does it automatically, WebStorm has built-in file watchers, otehrwise use gulp).
* Install TypeScript Definition manager for DefinitelyTyped 
http://www.tsdpm.com (https://github.com/Definitelytyped/tsd)

    npm install tsd -g

* Install: `npm install -g generator-gulp-webapp`.
* Run: `yo gulp-webapp`.
* Run `gulp` for building and `gulp watch` for preview - ensure that both commands work.
* Install required typings with `-rosa install` (resolve, overwrite, save, action) command, e.g. for angular: 

    tsd query angular -rosa install

* Run `npm install <pkg> --save` to install a package and save it to `package.json` config file.
* Initialize git repo. Run `npm install generator-gitignore -g` (https://www.npmjs.org/package/generator-gitignore). 
Then run `yo gitignore` (in the repo folder) and select files for yeoman and your IDE.
* Set up gulp for React https://github.com/yeoman/generator-gulp-webapp/blob/master/docs/recipes/react.md
* Set up gulp for TypeScript compilation if IDE doesn't do that for you.
* Commit the stub.
* TODO documentation, description...
