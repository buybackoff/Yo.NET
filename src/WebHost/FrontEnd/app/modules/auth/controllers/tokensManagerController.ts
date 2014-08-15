'use strict';
angular.module('authModule')
    .controller('tokensManagerController', ['$scope', 'tokensManagerService',
        function ($scope, tokensManagerService) {

            $scope.refreshTokens = [];

            tokensManagerService.getRefreshTokens().then(function (results) {

                $scope.refreshTokens = results.data;

            }, error => {
                    alert(error.data.message);
                });

            $scope.deleteRefreshTokens = function (index, tokenid) {

                tokenid = encodeURIComponent(tokenid);

                tokensManagerService.deleteRefreshTokens(tokenid).then(function (results) {

                    $scope.refreshTokens.splice(index, 1);

                }, function (error) {
                        alert(error.data.message);
                    });
            }

}]);