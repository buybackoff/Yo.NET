'use strict';
angular.module('authModule')
    .controller('signupController', ['$scope', '$location', '$timeout', 'authService',
        ($scope, $location, $timeout, authService : IAuthService) => {

            $scope.savedSuccessfully = false;
            $scope.message = "";

            $scope.registration = {
                userName: "",
                password: "",
                confirmPassword: ""
            };

            $scope.signUp = () => {

                authService.saveRegistration($scope.registration)
                    .then(response => {
                        $scope.savedSuccessfully = true;
                        $scope.message = "Thank you for registering! You will now be redicted to the login page.";
                        startTimer();
                    },
                    response => {
                        var errors = [];
                        for (var key in response.data.modelState) {
                            for (var i = 0; i < response.data.modelState[key].length; i++) {
                                errors.push(response.data.modelState[key][i]);
                            }
                        }
                        $scope.message = "Oops! Failed to register due to:" + errors.join(' ');
                    });
            };

            var startTimer = () => {
                var timer = $timeout(() => {
                    $timeout.cancel(timer);
                    $location.path('/login');
                }, 2000);
            }

        }]);