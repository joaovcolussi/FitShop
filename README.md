# 📌 Projeto: FitShop com HTML, CSS e JavaScript
🗓️ [Data de início – Data de conclusão]
🔗 [Link para o repositório Github](https://github.com/joaovcolussi/FitShop)


## Iniciar o projeto:
```
npm install
```
```
npm run dev
```

## Descrição:




## Principais páginas e funcionalidades:

🛒 Tela de Produto e Tela de Carrinho

🔍 Tela de Resultado de Pesquisa

🧾 Tela de Cadastro e Edição de Produto

📂 Tela de Cadastro e Edição de Categoria

🛠️ Painel Administrativo

🧭 Navegação entre páginas e estrutura responsiva

💡 Interações com JavaScript para filtros, validações de formulários e manipulação do carrinho

## Tecnologias utilizadas:
HTML5, CSS3, JavaScript

##Frameworks:
Vue

##Backend:
C#


## Estrutura do Projeto:
```
├── public/
│   └── logoFitShop.svg
├── src/
│   ├── assets/
│   │   └── scss/
│   │       └── main.scss
│   ├── components/
│   │   ├── layout/
│   │   │   ├── Cabecalho.vue
│   │   │   └── Rodape.vue
│   │   └── ui/
│   │       ├── CarrinhoItem.vue
│   │       ├── CategoriaMenu.vue
│   │       └── ProdutoCard.vue
│   ├── pages/
│   │   ├── admin/
│   │   │   ├── Categorias.vue
│   │   │   ├── EditarCategoria.vue
│   │   │   ├── EditarProduto.vue
│   │   │   ├── Estoque.vue
│   │   │   ├── Produtos.vue
│   │   │   └── Admin.vue
│   │   ├── Busca.vue
│   │   ├── Carrinho.vue
│   │   ├── Home.vue
│   │   └── Produto.vue
│   ├── services/
│   │   └── api.ts
│   ├── stores/
│   │   └── carrinho.ts
│   ├── App.vue
│   ├── main.ts
│   └── style.css
└── index.html
```