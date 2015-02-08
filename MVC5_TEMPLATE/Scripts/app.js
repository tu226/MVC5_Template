var app = angular.module("app", []);


app.controller("users", ["$scope","$http", function ($scope,$http) {

    $scope.main=true;
    $scope.edit = false;
    $scope.editUser = null;
    $scope.users = [];
    $scope.roles = [];

    $http.get("/roles/roles").success(function (data) {
        $scope.roles = data;
    });

    $http.get("/users/users").success(function (data) {
        $scope.users = data;
    });

    $scope.startEdit = function (user) {
        $scope.main = false;
        $scope.edit = true;
        $scope.editUser=user;
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

}]);


