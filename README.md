# 📝 WhatsApp To-Do Manager

> Um gerenciador de tarefas simples e eficiente integrado ao WhatsApp, projetado para pessoas que buscam uma solução mais prática e integrada ao seu dia a dia.

## 🎯 Sobre o Projeto

O WhatsApp To-Do Manager é uma solução inovadora que permite aos usuários gerenciar suas tarefas diárias diretamente pelo WhatsApp. Desenvolvido pensando na simplicidade e na praticidade, o projeto aproveita o alto engajamento dos usuários brasileiros com o WhatsApp para oferecer uma experiência fluida de gerenciamento de tarefas.

## ✨ Funcionalidades

### Implementadas

- [x] Criação de tarefas 
- [x] Edição de tarefas existentes
- [x] Exclusão de tarefas
- [x] Exibir lista de tarefas pendentes
- [x] Sistema de notificações automáticas
- [x] Menu de ajuda com comandos disponíveis

### Ações Disponíveis

- Criar uma nova tarefa
- Editar uma tarefa existente
- Deletar uma tarefa
- Listar todas as tarefas
- Exibir instruções e comandos disponíveis
- Exibir os criadores do projeto WhatsToDo

## 🚀 Tecnologias  
- .NET 9x como Framework Principal
- ASP.NET para o backend
- API do WhatsApp para comunicação
- SQLite para armazenamento local

Este projeto utiliza as seguintes tecnologias:

## 📋 Pré-requisitos

- .NET 9.0 SDK ou superior
- Visual Studio 2022 ou Visual Studio Code
- SQLite3
- Conta do WhatsApp Business API

## 🔧 Instalação

1. Clone o repositório  
   
   ```bash
   git clone https://github.com/taylors42/WhatsTodo
   ```

2. Abra a solução no Visual Studio 
   
   ```bash
   cd WhatsTodo
   start WhatsTodo.sln
   ```

3. Restaure os pacotes NuGet
   
   ```bash
   dotnet restore
   ```

4. Configure o arquivo appsettings.json
   
   ```bash
   cp appsettings.example.json appsettings.json
   # Edite o arquivo appsettings.json com suas configurações
   ```

5. Execute o projeto
   
   ```bash
   dotnet run
   ```

Alternativamente, você pode abrir o projeto no Visual Studio e pressionar Ctrl + F5 para executar.

## 📊 Estrutura do Banco de Dados

### Tabela `todos`

- `id` (INTEGER) - ID da tarefa (PK)
- `title` (VARCHAR) - Título da tarefa
- `description` (TEXT) - Descrição da tarefa
- `notification_date` (DATE) - Data da notificação
- `notification_time` (TIME) - Hora da notificação
- `is_completed` (BOOLEAN) - Status de conclusão
- `created_at` (TIMESTAMP) - Data de criação
- `user_phone` (VARCHAR) - Número do usuário (FK)
- `completed_at` (TIMESTAMP) - Data e hora de conclusão

### Tabela `users`
- `phone` (VARCHAR(20)) - Número de telefone do usuário (PK)
- `created_at` (TIMESTAMP) - Data de criação do usuário

## 📝 Status do Projeto

### MVP

- [x] Estruturação do banco de dados
- [x] Configuração inicial do projeto
- [x] Implementação da API do WhatsApp
- [x] CRUD básico de tarefas
- [x] Sistema de notificações

## 🤝 Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 👥 Autores

* **Taylor S** - *Trabalho Inicial* - [Taylors42](https://github.com/taylors42)
* **Camily Z** - *Desenvolvedora* - [MilyZani](https://github.com/MilyZani)