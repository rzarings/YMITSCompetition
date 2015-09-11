app.controller('personInfoController', ['$scope','$location','personInfoService',function ($scope, $location, personInfoService) {

    //Function to Reset Scope variables
    function initialize() {
        $scope.teamLead = "";
        $scope.email = "";
        $scope.subscriptionId = "";
        $scope.teamName = "";
    };

    //function when we load the view
    $scope.$on('$viewContentLoaded', function () {
        $scope.button="submit";
        var temp = personInfoService.getPage().then(function (data) {
            var obj = JSON.parse(data);
            if (obj.Status == 0)
                $location.path('/index');
            else if (obj.Status == 1) {
                $location.path('/logged');
            }
            else if (obj.Status == 2) {
                $location.path('/finished');
            }
            if (!$scope.$$phase) $scope.$apply();
        }, function (reason) {
            
        }).catch(function (error) {
            var obj = JSON.parse(error.message);
            alert(obj.Message);
        }

        );


    });

    //Function to Submit the form 
    $scope.submitForm = function () {

        if ($scope.TeamValidationText != "Fetching Data... Wait before submitting please") {
            var r = confirm($scope.TeamValidationText);
            if (r == true) {
                var Person = {};
                Person.Email = $scope.email;
                Person.TeamName = $scope.teamName;
                var promisePost = personInfoService.postInfo(Person).then(function () {
                    $location.path('/logged');
                }).catch(function (error) {
                  
                    var obj = JSON.parse(error.message);
                    alert(obj.Message);
                }
                );
            } else {

            }
        } else {
            alert("please wait that your team name is validated");
        }
     
    };

    //Function to Cancel Form
    $scope.cancelForm = function () {

        initialize();
    };


    $scope.teamNameValidation = function () {
        $scope.button.disabled = true;
        $scope.TeamValidationText = "Fetching Data... Wait before submitting please";
        var teamName = $scope.teamName;
        var result = personInfoService.teamNameAvailable(teamName).then(function (data) {

            var obj = JSON.parse(data);
  
            if (obj.Number == 0) {
                $scope.button = "create a new team";
                $scope.TeamValidationText = "You will create your own team, and get your own prize!";
            }
            else {
                $scope.button = "Join an existing team";
                $scope.TeamValidationText = "You will join another team! And contribute to a friend's prize. You won't get any prizes if you select this option!";
            }
        }     
        ).catch(function (error) {
            var obj = JSON.parse(error.message);
            alert(obj.Message);
            $scope.teamAvailable = "";
        });
    }; 
}]);

//controller for the logged view
app.controller('loggedController', ['$scope', '$location', 'loggedService', function ($scope, $location, loggedService) {
    
    //navigation on the logged view, update the guys there
    $scope.$on('$viewContentLoaded', function () {
        var temp = loggedService.getNumberOfPersInTeam().then(function (data) {         
            var obj = JSON.parse(data);
            $scope.numberPersonInTeam = obj.NumberUsersInTeam;
            $scope.numberOfUsersValidated = obj.NumberOfUsersValidated;
            $scope.personVerified= obj.PersonVerified;
            $scope.teamLead = obj.TeamLead;
            $scope.teamName = obj.TeamName;
          
        }
        , function (reason) {
            var obj = JSON.parse(reason.message);
            alert(obj.Message);
        }).catch(function (error) {

            var obj = JSON.parse(error.message);
            alert(obj.Message);
        }

        );
    });
    
    //function for the submit button
    $scope.clickButton = function () {

        var r = confirm("You are about to reedeam your prize. Please be aware that by reedeam your price your team will be locked and you want be able to add additional users to your team");
        if (r == true) {
            var promisePost = loggedService.finishContest().then(function () {
                $location.path('/finished');
            }).catch(function (error) {

                var obj = JSON.parse(error.message);
                alert(obj.Message);
            }

            );
        }
    };

    $scope.getNbPplLeft = function () {
        if ($scope.numberOfUsersValidated < 6) {
            return 6 - $scope.numberOfUsersValidated;
        } else if ($scope.numberOfUsersValidated < 12) {
            return 12 - $scope.numberOfUsersValidated;
        } else if ($scope.numberOfUsersValidated < 18) {
            return 18 - $scope.numberOfUsersValidated;
    }
    }
        
}]);

//controller for the finished view
app.controller('finishedController', ['$scope', function ($scope) {

    

}]);