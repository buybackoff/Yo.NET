'use strict';
var yoModule = angular.module('yoModule', ['ngSanitize', 'ui.router']);


yoModule.config([
    '$stateProvider', function ($stateProvider) {
        $stateProvider.state("yo", {
            url: "/yo",
            controller: "yoController",
            templateUrl: "/modules/yo/views/yo.html"
        });
    }]).factory('yoDataService', [
    '$http', function ($http) {
        // that we have access to YoResponse automatically
        // after running 'gulp servertypes'
        // !!! WebEssentials 2.2 disabled .d.ts compilation, should stay with 2.1
        var req = { message: '', name: '', withHistory: true };
        return {
            data: $http.get('/api/yo/', req),
            yoHubProxy: $.connection.yoHub
        };
    }]).controller("yoController", [
    '$scope', 'yoDataService', function ($scope, dataService) {
        $scope.state = {
            totalCount: 0,
            userCount: 0,
            messages: Array(),
            name: "",
            messageText: ""
        };
        $scope.state.messages.push("Yo!");
        $scope.state.messages.push("Say something and click, yo");
        dataService.data.success(function (data) {
            $scope.state.totalCount = data.allYos;
            $scope.state.userCount = data.userYos;
            //$scope.state.messages = data.history;
        });

        $scope.click = function () {
            dataService.yoHubProxy.server.yo($scope.state.name, $scope.state.messageText).done(function (res) {
                console.log('Done: ' + res);
            }).fail(function () {
                return console.log('Fail: ' + $scope.state.messageText);
            });
            $scope.state.messageText = "";
        };
    }]).filter('reverse', function () {
    return function (items) {
        return items.slice().reverse();
    };
}).run([
    'yoDataService', function (dataService) {
        $.connection.hub.start().done(function () {
            console.log("Now connected!");
        }).fail(function () {
            alert("Could not Connect!");
        });

        var yoScope;

        window.addEventListener('load', function () {
            // cast to any when too lazy to write interfaces
            yoScope = angular.element(document.getElementById("yoApp")).scope();
        });

        dataService.yoHubProxy.client.setAllYos = function (totalCount) {
            yoScope.$apply(function () {
                return yoScope.state.totalCount = totalCount;
            });
            //console.log('All Yos: ' + totalCount);
        };

        dataService.yoHubProxy.client.setMyYos = function (userCount) {
            yoScope.$apply(function () {
                return yoScope.state.userCount = userCount;
            });
        };

        dataService.yoHubProxy.client.addMessage = function (messageLine) {
            yoScope.$apply(function () {
                yoScope.state.messages.push(messageLine);
            });
            //console.log('' + name + ': ' + messageLine);
        };
    }]);
//# sourceMappingURL=yo.js.map
