TodoMVC with ReactJS and TypeScript
================

Steps
--------------

* Run `yo gulp-webapp`

* Run `npm install --save-dev gulp-react`
* Run `bower install --save react`
* Run `npm install react-typescript-definitions --save` which looks like the most updated version
of React definitions (https://github.com/wizzard0/react-typescript-definitions).

Translate the tutorial http://facebook.github.io/react/docs/flux-todo-list.html to TypeScript.

* Run `bower install es6-promise --save` to install a polyfill for ES6-style Promises 
and save the dependency (https://github.com/jakearchibald/es6-promise). 
Then run `tsd query es6-promises -rosa install`.
* Run `bower install todomvc-common --save`
