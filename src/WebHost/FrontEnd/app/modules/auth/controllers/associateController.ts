'use strict';


interface IAssociateControllerScope extends ng.IScope {
    savedSuccessfully: boolean;
    message: string;
    registerData: { userName;provider;externalAccessToken };
    registerExternal: () => void
}

angular.module('authModule')
    .controller('associateController',
    ['$scope', '$location', '$timeout', 'authService',
        ($scope: IAssociateControllerScope, $location : ng.ILocationService, $timeout : ng.ITimeoutService, authService: IAuthService) => {

            $scope.savedSuccessfully = false;
            $scope.message = "";

            $scope.registerExternal = () => {

                authService.registerExternal($scope.registerData)
                    .then(response => {

                        $scope.savedSuccessfully = true;
                        $scope.message = "User has been registered successfully, you will be redicted to orders page in 2 seconds.";
                        startTimer();

                    },
                    response => {
                        var errors = [];
                        for (var key in response.modelState) {
                            errors.push(response.modelState[key]);
                        }
                        $scope.message = "Failed to register user due to:" + errors.join(' ');
                    });
            };

            var startTimer = () => {
                var timer = $timeout(() => {
                    $timeout.cancel(timer);
                    $location.path('/');
                }, 2000);
            }

        }]);