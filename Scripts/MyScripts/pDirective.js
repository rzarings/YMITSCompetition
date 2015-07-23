app.directive('personForm',
    function () {

        return {
            restrict: 'E',
            templateUrl: '/Scripts/MyScripts/pTemplate.html'
        }

    });
/*    
app.directive('teamNameValidator', ['$http', function ($http) {
    return {
        require: 'ngModel',
        link: function ($scope, element, attrs, ngModel) {
            ngModel.$asyncValidators.teamNameValidator = function (username) {
                
                return $http.get('/api/Users/GetTeamNameAvailable?user=' + username).
                   success(function (data, response, headers, status) {
                       $scope.button = "create a new team";
                   }).
                   error(function (data, response) {
                       $scope.button = "Join an existing team";
                   });

            };
            
        }
    }
}
    
]);
*/
