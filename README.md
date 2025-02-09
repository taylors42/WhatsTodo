# 📝 WhatsApp To-Do Manager

> Um gerenciador de tarefas simples e eficiente integrado ao WhatsApp, projetado para pessoas que buscam uma solução mais prática e integrada ao seu dia a dia.

## 🎯 Sobre o Projeto

O WhatsApp To-Do Manager é uma solução inovadora que permite aos usuários gerenciar suas tarefas diárias diretamente pelo WhatsApp. Desenvolvido pensando na simplicidade e na praticidade, o projeto aproveita o alto engajamento dos usuários brasileiros com o WhatsApp para oferecer uma experiência fluida de gerenciamento de tarefas.

## ✨ Funcionalidades

### Implementadas

- [x] Criação de tarefas com título, data, hora e descrição
- [x] Listagem de tarefas pendentes
- [ ] Exclusão de tarefas
- [ ] Sistema de notificações automáticas
- [x] Menu de ajuda com comandos disponíveis

### Comandos Disponíveis

- `/addtask`: Cria uma nova tarefa
- `/edittask`: Editar uma tarefa
- `/listtask`: Lista todas as tarefas futuras
- `/creditos`: Criadores do projeto To-Do
- `/help`: Exibe instruções e comandos disponíveis

## 🚀 Tecnologias

Este projeto utiliza as seguintes tecnologias:

- SQLite para armazenamento local
- API do WhatsApp para comunicação
- ASP.NET para o backend

## 📋 Pré-requisitos

- .NET 9.0 SDK ou superior
- Visual Studio 2022 ou Visual Studio Code
- SQLite3
- Conta do WhatsApp Business API

## 🔧 Instalação

1. Clone o repositório
   
   ```bash
   git clone https://github.com/seu-usuario/whatsapp-todo-manager.git
   ```

2. Abra a solução no Visual Studio
   
   ```bash
   cd whatsapp-todo-manager
   WhatsAppToDoManager.sln
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

5. Execute as migrações do banco de dados
   
   ```bash
   dotnet ef database update
   ```

6. Execute o projeto
   
   ```bash
   dotnet run
   ```

Alternativamente, você pode abrir o projeto no Visual Studio e pressionar F5 para executar em modo debug.

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

## 📝 Status do Projeto

### MVP

- [x] Estruturação do banco de dados
- [x] Configuração inicial do projeto
- [x] Implementação da API do WhatsApp
- [ ] CRUD básico de tarefas
- [ ] Sistema de notificações

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

## 🙏 Agradecimentos

* WhatsApp Business API
* Comunidade Open Source
* Todos os contribuidores deste projeto
