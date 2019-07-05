QUnit.module('Chat Check User');

QUnit.test('Test valid user and return valid',
    function(assert) {

        //Arrange
        var inputUsername = "Bill";
        //Act
        var output = checkUser(inputUsername);
        //Assert
        assert.notOk(output.valid);
        

    });