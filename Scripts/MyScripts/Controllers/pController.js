app.controller('personInfoController', ['$scope','$location','personInfoService',function ($scope, $location, personInfoService) {

    //Function to Reset Scope variables
    function initialize() {
        $scope.TeamLead = "";
        $scope.Email = "";
        $scope.SubscriptionId = "";
        $scope.TeamName = "";
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
                Person.Email = $scope.Email;
                Person.TeamName = $scope.TeamName;
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
        var teamName = $scope.TeamName;
        var result=personInfoService.teamNameAvailable(teamName).then (function(data){
            if (data == true) {
                $scope.button = "create a new team";
                $scope.TeamValidationText = "You will create your own team, and get your own prize!";
            }
            else {
                $scope.button = "Join an existing team";
                $scope.TeamValidationText = "You will join another team! And contribute to a friend's prize. You won't get any prizes if you select this option!";
            }
        }     
        ).catch(function (error) {
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
            $scope.NumberOfUsersValidated = obj.NumberOfUsersValidated;
            $scope.personVerified= obj.PersonVerified;
            $scope.teamLead = obj.TeamLead;
          
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

        var r = confirm("You will now submit your team to get your prize. Past this point, your team will be locked. By accepting, you accept that Microsoft contact you per mail in the scope of delivering your prize");
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
        if ($scope.NumberOfUsersValidated < 6) {
            return 6 - $scope.NumberOfUsersValidated;
        } else if ($scope.NumberOfUsersValidated < 12) {
            return 12 - $scope.NumberOfUsersValidated;
        } else if ($scope.NumberOfUsersValidated < 18) {
        return 18 - $scope.NumberOfUsersValidated;
    }
    }
        
}]);

//controller for the finished view
app.controller('finishedController', ['$scope', function ($scope) {

    

}]);