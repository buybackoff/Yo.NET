TypeScript with ReactJS and AngularJS
===========

Preparation
-----------
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
* Typings from `tsd` should be added to .gitignore as `typings/`.
* Set up gulp for React https://github.com/yeoman/generator-gulp-webapp/blob/master/docs/recipes/react.md
* Set up gulp for TypeScript if IDE doesn't do that for you.
* Commit the stub.

