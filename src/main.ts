import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import { createPinia } from 'pinia'
import App from './App.vue'
import './assets/scss/main.scss'

// Importação de páginas
import Home from './pages/Home.vue'
import Busca from './pages/Busca.vue'
import Produto from './pages/Produto.vue'
import Carrinho from './pages/Carrinho.vue'
import Admin from './pages/Admin.vue'
import AdminProdutos from './pages/admin/Produtos.vue'
import AdminCategorias from './pages/admin/Categorias.vue'
import AdminEstoque from './pages/admin/Estoque.vue'
import AdminEditarProduto from './pages/admin/EditarProduto.vue'
import AdminEditarCategoria from './pages/admin/EditarCategoria.vue'

const routes = [
  { path: '/', component: Home },
  { path: '/busca', component: Busca },
  { path: '/produto/:id', component: Produto },
  { path: '/carrinho', component: Carrinho },
  { path: '/admin', component: Admin },
  { path: '/admin/produtos', component: AdminProdutos },
  { path: '/admin/categorias', component: AdminCategorias },
  { path: '/admin/estoque', component: AdminEstoque },
  { path: '/admin/produto/:id?', component: AdminEditarProduto },
  { path: '/admin/categoria/:id?', component: AdminEditarCategoria }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

const pinia = createPinia()
const app = createApp(App)

app.use(router)
app.use(pinia)
app.mount('#app')