
angular.module('authModule')
    .controller('authController', ['$scope', '$window', 'authService',
        ($scope, $window : ng.IWindowService, authService: IAuthService) => {
            $scope.isAuth = authService.isAuth;
            $scope.currentAuthNames = authService.getAuthNames();
            $scope.getAuthNames = authService.getAuthNames;
            $scope.refillAuthNames = authService.refillAuthNames;
            $scope.login = authService.login;
            $scope.logout = authService.logout;
            $scope.getPathname = () => { return $window.location.pathname; };
        }]);
 