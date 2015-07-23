app.service("personInfoService", function ($http,$q) {

    this.postInfo = function (userDTO) {
        var dfrd = $q.defer();
        var req = $http.post('/api/Users/PostNewUser', userDTO).success(
            function (data) {
                dfrd.resolve(data);
            }).error(function (error) {
                dfrd.reject(error);
            });
        return dfrd.promise;
    };

    this.teamNameAvailable = function (teamName) {
        var dfrdasd = $q.defer();
        var reqe = $http.get('/api/Users/TeamFree?teamName='+teamName).success(
         function (data) {
             dfrdasd.resolve(data);
         }).error(function (error) {
             dfrdasd.reject(error);
         });
        return dfrdasd.promise;

    };

    this.getPage = function () {
        var dfrd = $q.defer();
        $http.get('/api/Users/GetCurrentPageStatus').success(
            function (data) {
               dfrd.resolve(data);
            }).error(function (error) {
                dfrd.reject(error);
            });
        return dfrd.promise;
    };
});


app.service("loggedService", function ($http, $q) {
    this.getNumberOfPersInTeam = function () {
        var dfrd = $q.defer();
        $http.get("/api/Users/getTeamUserNumber").success(
            function (data) {
                dfrd.resolve(data);
            }).error(function (error) {
                dfrd.reject(error);
            });
        return dfrd.promise;
    };

    this.finishContest = function () {
        var dfrd = $q.defer();
        var req = $http.get('/api/Users/FinishContest').success(
            function (data) {
                dfrd.resolve(data);
            }).error(function (error) {
                dfrd.reject(error);
            });
        return dfrd.promise;
    };
});