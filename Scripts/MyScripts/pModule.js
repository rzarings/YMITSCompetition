var app;
(function () {
    app = angular.module("personInfoModule", ["ngMessages","ng","ui.bootstrap","ngRoute"]);
    //"ui.bootstrap"
})();

app.config(function ($routeProvider) {
    $routeProvider
       .when('/index', {
           templateUrl: 'Scripts/MyScripts/pTemplate.html',
           controller: 'personInfoController'
       })
       .when('/logged', {
           templateUrl: 'Scripts/MyScripts/pLogged.html',
           controller: 'loggedController'
       })
         .when('/finished', {
             templateUrl: 'Scripts/MyScripts/pFinished.html',
             controller: 'finishedController'
         })
       .otherwise({
           redirectTo: '/index'
       });
});


