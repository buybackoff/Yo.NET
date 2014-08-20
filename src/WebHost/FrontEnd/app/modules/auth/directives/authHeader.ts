
angular.module('authModule')
    .directive('authHeader', ['$window', 'authService',
        ($window: ng.IWindowService, authService: IAuthService) => {

            function link(scope, element, attrs) {
                scope.isAuth = authService.isAuth;
                scope.currentAuthNames = authService.getAuthNames();
                scope.getAuthNames = authService.getAuthNames;
                scope.refillAuthNames = authService.refillAuthNames;
                scope.login = authService.login;
                scope.logout = authService.logout;
                scope.getPathname = () => { return $window.location.pathname; };
            }
            var directive: ng.IDirective =
                {
                    restrict: 'EAC',
                    replace: true,
                    transclude: false,
                    scope: {},
                    templateUrl: '/modules/auth/views/authHeader.html',
                    link: link
                }
        return directive;
        }]);