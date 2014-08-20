angular.module('authModule').controller('authController', [
    '$scope', '$window', 'authService',
    function ($scope, $window, authService) {
        $scope.isAuth = authService.isAuth;
        $scope.currentAuthNames = authService.getAuthNames();
        $scope.getAuthNames = authService.getAuthNames;
        $scope.refillAuthNames = authService.refillAuthNames;
        $scope.login = authService.login;
        $scope.logout = authService.logout;
        $scope.getPathname = function () {
            return $window.location.pathname;
        };
    }]);
//# sourceMappingURL=authController.js.map
