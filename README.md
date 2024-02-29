# Validador de senhas 

Uma API que valida senhas com base em alguns critérios.

---

# Critérios de validação

- Pelo menos nove caracteres
- Pelo menos um dígito
- Pelo menos uma letra minúscula
- Pelo menos uma letra maiúscula
- Pelo menos um caractere especial
- Os seguintes caracteres são considerados especiais: !@#$%^&*()-+
- Não pode haver caracteres redundantes dentro do conjunto
- Espaços em branco não são considerados caracteres

---

# Exemplos de validação

```c#
IsValid("") // false  
IsValid("aa") // false  
IsValid("ab") // false  
IsValid("AAAbbbCc") // false  
IsValid("AbTp9!foo") // false  
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

---

# Como executar

Abrir a solução no Visual Studio e executar o projeto ao clicar no botão de executar deveria ser o suficiente para compilar e executar o projeto. <br>
Além disso, é esperado que o Swagger seja aberto no seu navegador principal. <br>
Com o Swagger aberto, clique na única rota "api/v1/Password" e então clique no botão "Try it out". <br>
Depois disso, digite qualquer senha no campo textbox e clique no botão "Execute". <br>

---

# Sobre a solução

Há diversas maneiras de validar uma senha. A primeira maneira que eu pensei foi usando Regex. <br>
Entretanto, o objetivo desse teste é me desafiar e avaliar a minha familiaridade com boas práticas de SOLID, Clean Code e design de APIs. <br>
Se eu usar Regex, eu não abraço o desafio real e não deixo abertura para a avaliação da minha experiência com boas práticas de programação. <br>
Por isso, nesse projeto, não estou usando Regex. <br>
<br>
Respeitando os princípios Single Responsibility e Dependency Injection, resolvi não construir toda a lógica de validação na classe Controller, ou seja, eu isolei a lógica de validação de senha em uma classe separada, a qual eu nomeei de "ValidationService". <br>
É uma convenção entre os programadores que não deve ser escrito muito código dentro da Controller. O processamento deve ser quase sempre separado em classes Services. <br>
A responsabilidade única da Controller é processar requisições e respostas HTTP. A regra de negócios, como a validação de uma senha, pode possuir um processamento separado em uma outra classe (serviço). <br>
Aproveitando, o serviço ValidationService foi injetado no Controller, evitando a instância direta de um objeto na classe Controller, ou seja, evitando um alto acoplamento e facilitando manutenções futuras. <br>
<br>
Cada critério de validação da senha, dentro do ValidationService, foi separado como um método, pois acho que o nome do método é mais legível do que um IF. Acredito que separar os critérios em métodos deixa o código mais limpo do que separar em vários IFs. <br>
De maneira geral, eu vi grande utilidade no LINQ e nos métodos prontos do char e da string para realizar as validações. O retorno é uma tabela verdade do tipo AND. <br>
Em outras palavras, todos os métodos de validação precisam retornar TRUE para que a senha seja válida. <br>
<br>
No Controller, é retornada uma resposta HTTP de status code 200 para indicar que a senha é válida. Se for inválida (reprovou na validação do ValidationService), é retornada uma resposta HTTP de status code 400 (erro do cliente por enviar uma senha inválida). <br>

---

# Tratamento de erros

Quando o cliente envia uma senha inválida, isto é, quando a senha não passa em um dos métodos de validação anteriormente explicados, isso é considerado um erro do cliente. <br>
O status code 4xx do HTTP é dedicado a erros do cliente. O status code 400 é para requisições ruins (bad requests), isto é, quando o cliente envia uma requisição HTTP contendo o conteúdo errado, como uma senha inválida. <br>
Para cada momento que a senha enviada pelo cliente não passa em uma regra de validação, o servidor envia uma resposta HTTP possuindo o status code 400, enviando o valor "false" em seu corpo. <br>

Ademais, também há a possibilidade de quando o usuário testar a aplicação através do Swagger, ele envia um valor nulo como uma senha. <br>
Para evitar esse problema, a exceção NullReferenceException é tratada: <br>


```c#
catch (NullReferenceException nrex)
{
   return BadRequest(false);
}
```

Mesmo assim, a aplicação ainda anuncia que a senha é inválida. <br>

Para tratar todos os casos possíveis de exceção, garantindo que a aplicação jamais poderá ser fechada abruptamente, um tratamento de exceção genérico é escrito, assumindo que esse seja um erro do servidor: <br>

```c#
catch (Exception ex)
{
   return StatusCode(500);
}
```

É importante enfatizar que, nesse caso, a exceção deveria ser salva dentro de um arquivo de log, e desde que esse erro poderia ser qualquer coisa (e desconhecido), não é trivial definir uma mensagem específica para o usuário. <br>
A aplicação front-end deveria disparar uma mensagem genérica ao usuário ao ler um status code 500 dentro de uma resposta HTTP.

---


# Better Password Validator 
An API that validates passwords basing on some criteria.

---

# Validation criteria

- At least nine characters
- At least one number
- At least one lower case letter
- At least one upper case letter
- At least one special character
- The following characters must be considered as special: !@#$%^&*()-+
- There must not be any redundant characters within the set
- White or blank spaces must not be considered valid characters

---

# Validation examples

```c#
IsValid("") // false  
IsValid("aa") // false  
IsValid("ab") // false  
IsValid("AAAbbbCc") // false  
IsValid("AbTp9!foo") // false  
IsValid("AbTp9!foA") // false
IsValid("AbTp9 fok") // false
IsValid("AbTp9!fok") // true
```

---

# How to execute

Opening the solution on Visual Studio and running the project by clicking on the "run" button should be enough to build and run the project. <br>
Moreover, it is expected that Swagger opens on your main browser. <br>
With Swagger open, click on the single route "api/v1/Password" and then click on "Try it out". <br>
Afterwards, type any password on the textbox field and click on "Execute" button. <br>

---

# About the solution

There are several ways to validate a password. The first way I thought of was using Regex. <br>
However, the goal of this test is to challenge myself and assess my familiarity with SOLID principles, Clean Code, and API design. <br>
If I use Regex, I don't embrace the real challenge and don't leave room for evaluating my experience with programming best practices. <br>
That's why, in this project, I'm not using Regex. <br>
<br>
Respecting the Single Responsibility and Dependency Injection principles, I decided not to build the entire validation logic in the Controller class, meaning I isolated the password validation logic into a separate class, which I named "ValidationService". <br>
It's a convention among programmers that not much code should be written inside the Controller. Processing should almost always be separated into Service classes. <br>
The sole responsibility of the Controller is to process HTTP requests and responses. Business rules, such as password validation, can have separate processing in another class (service). <br>
Additionally, the ValidationService was injected into the Controller, avoiding direct instantiation of an object in the Controller class, which means avoiding high coupling and facilitating future maintenance. <br>
<br>
Each password validation criterion within the ValidationService was separated into a method because I believe the method name is more readable than an IF statement. I think separating the criteria into methods makes the code cleaner than separating them into multiple IF statements. <br>
Overall, I found great utility in LINQ and the built-in methods of char and string for performing validations. The return is a truth table of the AND type. <br>
In other words, all validation methods need to return TRUE for the password to be valid. <br>
<br>
In the Controller, an HTTP response with status code 200 is returned to indicate that the password is valid. If it's invalid (failed in the ValidationService validation), an HTTP response with status code 400 is returned (client error for sending an invalid password). <br>


---

# Error handling

When the client sends an invalid password, i.e., when the password doesn't pass one of the validation methods previously explained, it is considered a client error. <br>
The HTTP 4xx status code is dedicated to client errors. The 400 status code is for bad requests, i.e., when the client sends an HTTP request containing wrong content, like an invalid password. <br>
For each time the password sent by the client does not pass a validation method, the server sends an HTTP response possessing 400 status code, sending the "false" value in its body. <br>

Furthermore, it is also possible that when the user tests the application through swagger, they send a null value for password. <br>
To avoid that problem, the NullReferenceException is caught and handled: <br>

```c#
catch (NullReferenceException nrex)
{
   return BadRequest(false);
}
```

Even so, the application still announces that the password is invalid. <br>

To catch all possible exception cases, making sure that the application might never shut down, a generic exception handling is placed, assuming that it is a server error: <br>

```c#
catch (Exception ex)
{
   return StatusCode(500);
}
```

It is important to point out that, in this case, the exception should be saved into a log file, and since this error could be anything (and unknown), it is not trivial to define a specific message to the user. <br>
The front-end application should raise a generic message to the user when reading a 500 HTTP status code in an HTTP response.


