# BudgetZen — Gestion de comptes (dépenses / revenus)

Application full-stack avec API ASP.NET Core (.NET 8) + EF Core + SQLite (dev) et frontend Vue 3 + Vite + TypeScript.

## Choix techniques (résumé)
- **Backend (.NET 8)** : architecture en couches (API → Application → Infrastructure → Domain) pour isoler la logique métier et faciliter les tests. DTOs systématiques pour ne jamais exposer les entités EF. Gestion d’erreurs uniforme via `ProblemDetails` middleware.
- **Sécurité** : JWT signé (HMAC SHA256) + hashing BCrypt. Chaque requête est filtrée par `UserId` (anti-IDOR). Validation d’entrée avec FluentValidation.
- **Frontend (Vue 3 + Tailwind)** : architecture par feature (auth, dashboard, accounts, categories, transactions, budgets). Stores Pinia, Axios interceptors, route guards. i18n FR et gestion du temps en UTC côté API.

> **Note** : Le refresh token n’est pas implémenté pour rester MVP, mais le JWT est correctement signé et expiré. Alternative recommandée : ajouter un couple `RefreshToken` + rotation + révocation en base.

---

## Arborescence
```
/backend
  /src (Domain, Application, Infrastructure, Api)
  /tests
/frontend
  /src (features, components, api, router, stores, types)
```

---

## Backend
### Démarrage
```bash
cd backend/src/Api
# Configurer la clé JWT (obligatoire pour prod)
# export Jwt__SigningKey="..."

# Lancer l'API
# dotnet run
```

### Migrations
```bash
cd backend/src/Api
# dotnet ef migrations add InitialCreate --project ../Infrastructure/Infrastructure.csproj
# dotnet ef database update
```

### Configuration
- `appsettings.json` : connexion SQLite, JWT, CORS.
- Variables d’environnement recommandées :
  - `ConnectionStrings__Default`
  - `Jwt__Issuer`, `Jwt__Audience`, `Jwt__SigningKey`

### Auth (seed dev)
Utilisateur seed (dev) :
- email : `demo@budgetzen.local`
- mot de passe : `ChangeMe123!`

### Exemples curl
```bash
# Register
curl -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"StrongPass123!"}'

# Login
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"StrongPass123!"}'

# Récupérer les comptes
curl -X GET http://localhost:5000/api/v1/accounts \
  -H "Authorization: Bearer <TOKEN>"

# Créer une transaction
curl -X POST http://localhost:5000/api/v1/transactions \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"accountId":"<ID>","categoryId":"<ID>","type":"Expense","amount":42.5,"description":"Café","occurredAtLocal":"2024-09-10T08:30:00"}'
```

---

## Frontend
### Démarrage
```bash
cd frontend
# Installer les dépendances
# npm install

# Lancer le front
# npm run dev
```

### Variables d’environnement
Créer `.env` si besoin :
```
VITE_API_BASE_URL=http://localhost:5000/api/v1
```

---

## Tests
```bash
cd backend/tests/ApiTests
# dotnet test
```

---

## API — Endpoints principaux
- **Auth**: `POST /api/v1/auth/register`, `POST /api/v1/auth/login`
- **Accounts**: `GET/POST/PUT/DELETE /api/v1/accounts`
- **Categories**: `GET/POST/PUT/DELETE /api/v1/categories`
- **Transactions**: `GET /api/v1/transactions` + filtres `fromUtc/toUtc/type/accountId/categoryId/q` + pagination `page/pageSize`
- **Budgets**: `GET/POST/PUT/DELETE /api/v1/budgets`
- **Dashboard**: `GET /api/v1/dashboard/summary?year=YYYY&month=MM`

---

## Remarques qualité
- **Pas de logique métier dans les controllers** : les controllers délèguent aux services applicatifs.
- **EF Core** : décimaux `decimal(18,2)`, index, `AsNoTracking` sur les lectures.
- **Logs** : ILogger intégré, sans données sensibles.

