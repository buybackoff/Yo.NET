angular.module('authModule').directive('authHeader', [
    '$window', 'authService',
    function ($window, authService) {
        function link(scope, element, attrs) {
            scope.isAuth = authService.isAuth;
            scope.currentAuthNames = authService.getAuthNames();
            scope.getAuthNames = authService.getAuthNames;
            scope.refillAuthNames = authService.refillAuthNames;
            scope.login = authService.login;
            scope.logout = authService.logout;
            scope.getPathname = function () {
                return $window.location.pathname;
            };
        }
        var directive = {
            restrict: 'EAC',
            replace: false,
            transclude: false,
            scope: {},
            templateUrl: '/modules/auth/views/authHeader.html',
            link: link
        };
        return directive;
    }]);
//# sourceMappingURL=authHeader.js.map
