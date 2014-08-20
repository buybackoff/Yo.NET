console.log('Hello, app.ts!');

angular.module('yoApp', ['templates', 'authModule', 'yoModule', 'ui.router'])
    .config(['$locationProvider', ($locationProvider) => {

        $locationProvider
            .html5Mode(true);

    }]);






