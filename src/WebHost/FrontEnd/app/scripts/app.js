console.log('Hello, app.ts!');

angular.module('yoApp', ['templates', 'authModule', 'yoModule', 'ui.router']).config([
    '$locationProvider', function ($locationProvider) {
        $locationProvider.html5Mode(true);
    }]);
//# sourceMappingURL=app.js.map
