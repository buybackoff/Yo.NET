console.log('Hello, app.ts!');
declare var introJs;
angular.module("yoApp", ['ngSanitize'])
    .factory('yoDataSrvc', [
        '$http', ($http: ng.IHttpService) => {
            // that we have access to YoResponse automatically
            // after running 'gulp servertypes'
            // !!! WebEssentials 2.2 disabled .d.ts compilation, should stay with 2.1
            var req: server.Yo = { message: '', name: '', withHistory: true }
            return {
                data: $http.post('/api/yo/', req)
            };
        }])
    .controller("yoCtrl", ['$scope', 'yoDataSrvc', ($scope, $dataSrvc) => {

        $scope.state = {
            totalCount: 0,
            userCount: 0,
            messages: Array<string>(),
            name: "",
            messageText: ""
        };
        $scope.state.messages.push("Yo!");
        $scope.state.messages.push("Say something and click, yo");
        $dataSrvc.data.success((data: server.YoResponse) => {
            $scope.state.totalCount = data.allYos;
            $scope.state.userCount = data.myYos;
            if (data.myYos == 1) {
                //introJs().setOptions({ 'exitOnOverlayClick': false, 'exitOnEsc': false });
                introJs().start();
            }
            $scope.state.messages = data.history;
        });

        $scope.click = () => {
            yoHubProxy.server.yo($scope.state.name, $scope.state.messageText)
                .done(res => { console.log('Done: ' + res); })
                .fail(() => console.log('Fail: ' + $scope.state.messageText));
            $scope.state.messageText = "";
        }

    }])
    .filter('reverse', () => items => items.slice().reverse());



$.connection.hub.start()
    .done(() => { /*yoHubProxy.server.yo('', '');*/ console.log("Now connected!"); })
    .fail(() => { alert("Could not Connect!"); });

interface IYoHub extends HubConnection {
    // YoHub Client functions: 
    client: {
        setAllYos: (totalCounter: number) => void;
        setMyYos: (userCounter) => void;
        addMessage: (messageLine) => void
    };
    // YoHub Server function: 
    server: {
        yo(name: string, message: string): any;
    };
}

// extend SignalR with the hub 
interface SignalR {
    yoHub: IYoHub;
}

var yoHubProxy = $.connection.yoHub;
var yoScope: any;

window.addEventListener('load', () => {
    // cast to any when too lazy to write interfaces
    yoScope = angular.element(document.getElementById("yoApp")).scope();
});

yoHubProxy.client.setAllYos = (totalCount) => {
    yoScope.$apply(() =>
        yoScope.state.totalCount = totalCount
        );
    //console.log('All Yos: ' + totalCount);
}

yoHubProxy.client.setMyYos = (userCount) => {
    yoScope.$apply(() =>
        yoScope.state.userCount = userCount
        );
}

yoHubProxy.client.addMessage = (messageLine) => {
    yoScope.$apply(() => {
        yoScope.state.messages.push(messageLine);
    });
    //console.log('' + name + ': ' + messageLine);
}





