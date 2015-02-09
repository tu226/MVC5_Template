var app = angular.module("app", []);


app.controller("users", ["$scope","$http", function ($scope,$http) {

    $scope.main=true;
    $scope.edit = false;
    $scope.editUser = null;
    $scope.users = [];
    $scope.roles = [];
    $scope.create = false;
    $scope.newUser = null;

    $http.get("/roles/roles").success(function (data) {
        $scope.roles = data;
    });

    $http.get("/users/users").success(function (data) {
        $scope.users = data;
    });


    //Edit an user
    $scope.startEdit = function (user) {
        $scope.main = false;
        $scope.edit = true;
        $scope.editUser=angular.copy(user);
    };

    $scope.saveUser = function (user) {

        $http.post("/users/save", user).success(function (data) {
            $http.get("/users/users").success(function (data) {
                $scope.users = data;
                $scope.editUser = null;
                $scope.edit = false;
                $scope.main = true;
            });

        });;
    };

    $scope.addRole = function (role) {

        $scope.editUser.roles.push(role);


    };

    $scope.cancelEdit = function () {
        $scope.editUser = null;
        $scope.edit = false;
        $scope.main = true;
    }

    $scope.filterRoles = function (role) {
        if ($scope.editUser == null) {
            return true;
        }
        var isIn = true;
        $scope.editUser.roles.forEach(function (item) {
            if (item.roleid == role.roleid) {
                isIn = false;
            }
        });
        return isIn;
    };

    $scope.removeRole = function (role) {
        var index = $scope.editUser.roles.indexOf(role);
        if (index > -1) {
            $scope.editUser.roles.splice(index,1);
        }

    }


    //Create an user
    $scope.startCreate = function () {
        $scope.main = false;
        $scope.create = true;
        $scope.newUser = {username:null,roles:[]};
    }

    $scope.cancelCreate = function () {
        $scope.main = true;
        $scope.create = false;
        $scope.newUser = null;
        $("#newUserLabel").css("color", "black");
    }

    $scope.addRoleToNewUser = function (role) {
        $scope.newUser.roles.push(role);
    }

    $scope.removeRoleToNewUser = function (role) {
        var index = $scope.newUser.roles.indexOf(role);
        if (index > -1) {
            $scope.newUser.roles.splice(index, 1);
        }

    }

    $scope.filterRolesNewUser = function (role) {
        if ($scope.newUser == null) {
            return true;
        }
        var isIn = true;
        $scope.newUser.roles.forEach(function (item) {
            if (item.roleid == role.roleid) {
                isIn = false;
            }
        });
        return isIn;
    };


    $scope.createUser = function () {
        if ($scope.newUser.username == "" | $scope.newUser.username == null) {
            $("#newUserLabel").css("color", "red");
        }
        else {
            $("#newUserLabel").css("color", "black");
            $http.post("/users/register",{username:$scope.newUser.username,password:$scope.newUser.password}).success(function(data){
                console.log(data);
            });
        }
    }

}]);


