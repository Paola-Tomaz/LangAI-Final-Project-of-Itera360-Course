# 🌍 LangAI

LangAI é uma plataforma interativa de aprendizado de idiomas com exercícios gerados por Inteligência Artificial. O projeto combina uma interface envolvente com personagens personalizados, geração de perguntas em tempo real via IA e um sistema de progresso com XP e ranking.

---

## 🚀 Funcionalidades

- Escolha do idioma com personagens únicos (Klaus 🇩🇪, Lola 🇬🇧, Sebastian 🇫🇷, Miyuki 🇯🇵)
- Geração automática de perguntas via IA (Groq)
- Sistema de XP por acerto e barra de progresso por exercício
- Ranking em tempo real entre os usuários
- Upload de foto de perfil
- Cadastro, login e autenticação JWT
- Painel de admin para gerenciamento de usuários

---

## 🏗️ Arquitetura do Projeto

**Frontend**: React.js + CSS Modules  
**Backend**: .NET 6 Web API  
**Banco de Dados**: SQL Server  
**Integração IA**: Groq (modelo llama-3.3-70b-versatible)  
**Armazenamento de imagens**: AWS S3  
**Autenticação**: JWT + Claims

### 📦 Estrutura em camadas:

```
LangAI/
├── LangAI.API/           # ASP.NET Core Web API (Controllers, Services, DTOs)
├── LangAI.Aplicacao/     # Interfaces e regras de negócio
├── LangAI.Dominio/       # Entidades e enums
├── LangAI.Infraestrutura/ # Repositórios e contexto EF Core
└── frontend/             # Aplicação React
```

---

## 🧰 Tecnologias Utilizadas

| Tecnologia        | Função                         |
|-------------------|--------------------------------|
| React.js          | Interface de usuário           |
| ASP.NET Core Web API | Backend / API REST          |
| Entity Framework  | ORM para SQL Server            |
| AWS S3            | Upload e exibição de imagens   |
| Groq              |  Integração com IA             |
| JWT               | Autenticação e autorização     |
| react-hook-form + yup | Validação de formulários   |

---

## 🖥️ Como Executar Localmente

### 🔧 Requisitos

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download)
- [Node.js](https://nodejs.org/)
- [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- Conta no [Groq](https://console.groq.com) com API Key
- Conta AWS com bucket S3 e CORS configurado

---

### 📦 Backend (.NET)

```bash
cd LangAI.API
dotnet restore
dotnet ef database update
dotnet run
```

✅ API será exposta em: `https://localhost:7076`

---

### 🌐 Frontend (React)

```bash
cd langai-app
npm install
npm start
```

✅ App será aberto em: `http://localhost:3000`

---

## 🔑 Configuração da IA

No `appsettings.json`:

```json
"Groq": {
  "ApiKey": "SUA_API_KEY",
  "Model": "llama3-8b-8192",
  "BaseUrl": "https://api.groq.com/openai/v1/chat/completions"
}
```

---

## 🧪 Exemplo de uso da API

### 🔐 Login

`POST /Usuarios/Login`

```json
{
  "email": "paola@email.com",
  "senha": "123456"
}
```

Retorna um token JWT para usar nas próximas chamadas.

---

### 🤖 Gerar Pergunta

`POST /api/ai/gerar?idioma=ingles`

Retorna:

```json
{
  "sucesso": true,
  "pergunta": {
    "pergunta": "Qual é a tradução de 'dog'?",
    "opcoes": ["Dog", "Cat", "Fish", "Bird"],
    "respostaCorreta": "Dog"
  }
}
```

---

## 🧠 Decisões Técnicas

- IA foi integrada via Groq para controle e performance
- O XP é incrementado apenas após respostas corretas
- Ranking é consultado via `GET /Usuarios/ListarTodos` e ordenado por XP
- O sistema usa Claims JWT para garantir segurança e acesso aos dados do usuário
- O frontend trata falhas da IA com fallback para perguntas padrão

---

## ❗ Desafios enfrentados

- **Evitar repetição de perguntas da IA**  
  → Resolvido com prompts mais dinâmicos e validação no backend

- **Integração direta com S3 (presigned URL)**  
  → Permitido via `PUT` com permissões temporárias

- **Sincronizar XP em tempo real entre `Exercicio.js` e `Ranking.js`**  
  → Solução via `useEffect` e atualização após cada acerto

---

## 🙋‍♀️ Autora

Desenvolvido por **Paola Tomaz Novato do Prado** 💙  
Projeto universitário com fins de aprendizado e demonstração.

---

## 📝 Licença

Este projeto é de uso livre para fins educacionais.
