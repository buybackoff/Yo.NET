ReactJS and TypeScript
===========

Steps
------
1. Prepare your environment by installing Node.js, npm, git, grunt, bower, gulp, [yeoman](http://yeoman.io/codelab/setup.html). 
Setup TypeScript compilation on save (VS does it automatically, WebStorm has built-in file watchers, otehrwise use gulp).
1. Create empty Web Site project with a hellow-world index.html.
1. Install TypeScript Definition manager for DefinitelyTyped 
http://www.tsdpm.com (https://github.com/Definitelytyped/tsd)

    npm install tsd -g
1. Install required typings with `-rosa install` (resolve, overwrite, save, action) command, e.g. for angular: 

    tsd query angular -rosa install

1. Run  `npm init` to initialize npm packages config and then enter the dialog options
1. Run `npm install <pkg> --save` to install a package and save it to `package.json` config file
1. Run  `bower init` to initialize bower packages config and then enter the dialog options

> Use `npm` for backend dependencies and bower for frontend. See http://stackoverflow.com/questions/21198977/difference-between-grunt-npm-and-bower-package-json-vs-bower-json
and http://stackoverflow.com/questions/18641899/difference-between-bower-and-npm for details.

1. Initialize git repo. Run `npm install generator-gitignore -g` (https://www.npmjs.org/package/generator-gitignore). 
Then run `yo gitignore` (in the repo folder) and select files for yeoman and your IDE. Commit the stub.
