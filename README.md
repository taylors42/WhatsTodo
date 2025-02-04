# 📝 WhatsApp To-Do Manager

> Um gerenciador de tarefas simples e eficiente integrado ao WhatsApp, projetado para pessoas que buscam uma solução mais prática e integrada ao seu dia a dia.

## 🎯 Sobre o Projeto

O WhatsApp To-Do Manager é uma solução inovadora que permite aos usuários gerenciar suas tarefas diárias diretamente pelo WhatsApp. Desenvolvido pensando na simplicidade e na praticidade, o projeto aproveita o alto engajamento dos usuários brasileiros com o WhatsApp para oferecer uma experiência fluida de gerenciamento de tarefas.

## ✨ Funcionalidades

### Implementadas
- [ ] Configuração de horários permitidos para notificações
- [ ] Criação de tarefas com título, data, hora e descrição
- [ ] Sistema de recorrência de tarefas
- [ ] Listagem de tarefas pendentes
- [ ] Exclusão de tarefas
- [ ] Sistema de notificações automáticas
- [ ] Menu de ajuda com comandos disponíveis

### Comandos Disponíveis
- `/novo` ou `criar tarefa`: Cria uma nova tarefa
- `ver tarefas`: Lista todas as tarefas futuras
- `editar horários` ou `/horario`: Configura horários permitidos
- `excluir tarefa`: Remove uma tarefa específica
- `/ajuda`: Exibe instruções e comandos disponíveis

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

### Tabela `users`
- `phone_number` (VARCHAR) - Número do telefone (PK)
- `start_time` (TIME) - Horário inicial para notificações
- `end_time` (TIME) - Horário final para notificações
- `created_at` (TIMESTAMP) - Data de criação
- `is_active` (BOOLEAN) - Status do usuário

### Tabela `todos`
- `id` (INTEGER) - ID da tarefa (PK)
- `user_phone` (VARCHAR) - Número do usuário (FK)
- `title` (VARCHAR) - Título da tarefa
- `description` (TEXT) - Descrição da tarefa
- `notification_date` (DATE) - Data da notificação
- `notification_time` (TIME) - Hora da notificação
- `is_recurring` (BOOLEAN) - Indica se é recorrente
- `recurrence_pattern` (VARCHAR) - Padrão de recorrência
- `is_completed` (BOOLEAN) - Status de conclusão
- `created_at` (TIMESTAMP) - Data de criação

## 📝 Status do Projeto

### MVP
- [x] Estruturação do banco de dados
- [ ] Configuração inicial do projeto
- [ ] Implementação da API do WhatsApp
- [ ] Sistema de configuração de horários
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

* **Taylor S** - *Trabalho Inicial* - [taylors42](https://github.com/taylors42)

## 🙏 Agradecimentos

* WhatsApp Business API
* Comunidade Open Source
* Todos os contribuidores deste projeto
