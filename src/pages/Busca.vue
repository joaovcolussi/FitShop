<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRoute } from 'vue-router'
import ProdutoCard from '../components/ui/ProdutoCard.vue'
import api_whatsapp from '../services/api'

const route = useRoute()
const produtos = ref<any[]>([])
const categorias = ref<any[]>([])
const carregando = ref(true)
const termoBusca = ref('')
const categoriaAtiva = ref('')

const buscarProdutos = async () => {
  carregando.value = true
  
  try {
    const filtro: { termo?: string, categoria?: string } = {}
    
    if (termoBusca.value) {
      filtro.termo = termoBusca.value
    }
    
    if (categoriaAtiva.value) {
      filtro.categoria = categoriaAtiva.value
    }
    
    const resultados = await api_whatsapp.getProdutos(filtro)
    produtos.value = resultados
  } catch (error) {
    console.error('Erro ao buscar produtos', error)
  } finally {
    carregando.value = false
  }
}

const carregarCategorias = async () => {
  try {
    const resultado = await api_whatsapp.getCategorias()
    categorias.value = resultado
  } catch (error) {
    console.error('Erro ao carregar categorias', error)
  }
}

const selecionarCategoria = (slug: string) => {
  categoriaAtiva.value = categoriaAtiva.value === slug ? '' : slug
  buscarProdutos()
}

watch(
  () => route.query,
  (query) => {
    if (query.termo) {
      termoBusca.value = query.termo as string
    }
    
    if (query.categoria) {
      categoriaAtiva.value = query.categoria as string
    }
    
    buscarProdutos()
  },
  { immediate: true }
)

onMounted(() => {
  carregarCategorias()
})
</script>

<template>
  <div class="busca">
    <div class="container">
      <div class="busca__cabecalho">
        <h1 class="busca__titulo">Produtos</h1>
        
        <form @submit.prevent="buscarProdutos" class="busca__form">
          <div class="campo busca__campo">
            <input 
              type="text" 
              v-model="termoBusca" 
              placeholder="Buscar produtos..." 
              class="campo__input"
            />
          </div>
          <button type="submit" class="botao busca__botao">Buscar</button>
        </form>
      </div>
      
      <div class="busca__layout">
        <aside class="busca__filtros">
          <div class="filtros">
            <h3 class="filtros__titulo">Categorias</h3>
            <ul class="filtros__lista">
              <li 
                v-for="categoria in categorias" 
                :key="categoria.id"
                class="filtros__item"
              >
                <button 
                  class="filtros__botao" 
                  :class="{ 'filtros__botao--ativo': categoriaAtiva === categoria.slug }"
                  @click="selecionarCategoria(categoria.slug)"
                >
                  {{ categoria.nome }}
                </button>
              </li>
            </ul>
          </div>
        </aside>
        
        <main class="busca__conteudo">
          <div v-if="carregando" class="busca__carregando">
            <p>Buscando produtos...</p>
          </div>
          
          <div v-else-if="produtos.length === 0" class="busca__vazio">
            <p>Nenhum produto encontrado.</p>
          </div>
          
          <div v-else class="grade">
            <ProdutoCard 
              v-for="produto in produtos" 
              :key="produto.id"
              :produto="produto"
            />
          </div>
        </main>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.busca {
  &__cabecalho {
    margin-bottom: var(--espacamento-grande);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__form {
    display: flex;
    gap: var(--espacamento-pequeno);
    max-width: 600px;
    
    .campo {
      flex: 1;
      margin-bottom: 0;
    }
  }
  
  &__layout {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--espacamento-grande);
    
    @media (min-width: 768px) {
      grid-template-columns: 250px 1fr;
    }
  }
  
  &__filtros {
    @media (max-width: 767px) {
      margin-bottom: var(--espacamento-medio);
    }
  }
  
  &__carregando, &__vazio {
    padding: var(--espacamento-grande);
    text-align: center;
    color: var(--cor-cinza-escuro);
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
  }
}

.filtros {
  background-color: var(--cor-branco);
  border-radius: var(--borda-raio);
  padding: var(--espacamento-medio);
  position: sticky;
  top: calc(70px + var(--espacamento-medio));
  
  &__titulo {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-medio);
    padding-bottom: var(--espacamento-pequeno);
    border-bottom: 1px solid var(--cor-cinza-claro);
  }
  
  &__lista {
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-pequeno);
    
    @media (max-width: 767px) {
      flex-direction: row;
      flex-wrap: wrap;
    }
  }
  
  &__botao {
    display: block;
    width: 100%;
    text-align: left;
    padding: var(--espacamento-pequeno);
    background: none;
    border: none;
    border-radius: var(--borda-raio);
    cursor: pointer;
    transition: var(--transicao-padrao);
    font-size: var(--fonte-tamanho-base);
    
    &:hover {
      background-color: var(--cor-cinza-claro);
    }
    
    &--ativo {
      background-color: var(--cor-primaria);
      color: var(--cor-branco);
      
      &:hover {
        background-color: var(--cor-primaria);
      }
    }
    
    @media (max-width: 767px) {
      width: auto;
    }
  }
}
</style>