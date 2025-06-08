<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ProdutoCard from '../components/ui/ProdutoCard.vue'
import api_whatsapp from '../services/api' 

const produtosDestaque = ref<any[]>([])
const produtosPromocoes = ref<any[]>([])
const produtosNovidades = ref<any[]>([])

const carregandoDestaques = ref(true)
const carregandoPromocoes = ref(true)
const carregandoNovidades = ref(true)

const carregarDadosDestaque = async () => {
  try {
    const produtosData = await api_whatsapp.getProdutos() 
    produtosDestaque.value = produtosData
  } catch (error) {
    console.error('Erro ao carregar produtos em destaque', error)
  } finally {
    carregandoDestaques.value = false
  }
}

const carregarPromocoes = async () => {
  try {
    const promocoesData = await api_whatsapp.getPromocoes()
    produtosPromocoes.value = promocoesData
  } catch (error) {
    console.error('Erro ao carregar promoções', error)
    produtosPromocoes.value = [] 
  } finally {
    carregandoPromocoes.value = false
  }
}

const carregarNovidades = async () => {
  try {
    const novidadesData = await api_whatsapp.getNovidades()
    produtosNovidades.value = novidadesData
  } catch (error) {
    console.error('Erro ao carregar novidades', error)
    produtosNovidades.value = [] 
  } finally {
    carregandoNovidades.value = false
  }
}

onMounted(() => {
  carregarDadosDestaque()
  carregarPromocoes()
  carregarNovidades()
})
</script>

<template>
  <div class="home">
    <div class="banner">
      <div class="banner__conteudo">
        <h1 class="banner__titulo">FitShop</h1>
        <p class="banner__subtitulo">Sua loja completa de produtos</p>
        <router-link to="/busca" class="botao botao--grande banner__botao">
          Ver produtos
        </router-link>
      </div>
    </div>
    
    <div class="container">
      <section class="secao">
        <div class="secao__cabecalho">
          <h2 class="secao__titulo">Produtos em destaque</h2>
          <router-link to="/busca" class="secao__link">Ver todos</router-link>
        </div>
        
        <div v-if="carregandoDestaques" class="carregando">
          <p>Carregando produtos em destaque...</p>
        </div>
        
        <div v-else-if="produtosDestaque.length > 0" class="produtos-carrossel">
          <div class="produtos-carrossel__container">
            <div 
              v-for="produto in produtosDestaque" 
              :key="`destaque-${produto.id}`"
              class="produtos-carrossel__item"
            >
              <ProdutoCard :produto="produto" />
            </div>
          </div>
        </div>
        <div v-else class="secao__conteudo">
          <p>Nenhum produto em destaque disponível no momento.</p>
        </div>
      </section>

      <section class="secao">
        <div class="secao__cabecalho">
          <h2 class="secao__titulo">Promoções</h2>
          </div>
        
        <div v-if="carregandoPromocoes" class="carregando">
          <p>Carregando promoções...</p>
        </div>
        
        <div v-else-if="produtosPromocoes.length > 0" class="produtos-carrossel">
          <div class="produtos-carrossel__container">
            <div 
              v-for="produto in produtosPromocoes" 
              :key="`promocao-${produto.id}`"
              class="produtos-carrossel__item"
            >
              <ProdutoCard :produto="produto" /> </div>
          </div>
        </div>
        <div v-else class="secao__conteudo">
          <p>Nenhuma promoção disponível no momento.</p>
        </div>
      </section>

      <section class="secao">
        <div class="secao__cabecalho">
          <h2 class="secao__titulo">Novidades</h2>
          </div>
        <div v-if="carregandoNovidades" class="carregando">
          <p>Carregando novidades...</p>
        </div>
        <div v-else-if="produtosNovidades.length > 0" class="produtos-carrossel">
          <div class="produtos-carrossel__container">
            <div 
              v-for="produto in produtosNovidades" 
              :key="`novidade-${produto.id}`"
              class="produtos-carrossel__item"
            >
              <ProdutoCard :produto="produto" />
            </div>
          </div>
        </div>
        <div v-else class="secao__conteudo">
          <p>Nenhuma novidade disponível no momento.</p>
        </div>
      </section>

      <section class="secao">
        <div class="cta">
          <div class="cta__conteudo">
            <h2 class="cta__titulo">Explore os produtos no nosso site</h2>
            <p class="cta__texto">
              Encontre tudo que você precisa em um só lugar.
            </p>
            <router-link to="/busca" class="botao cta__botao">
              Explorar catálogo
            </router-link>
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<style lang="scss" scoped>


.home {
  padding-top: 0;
}

.banner {
  height: 500px;
  background: linear-gradient(rgba(0, 0, 163, 0.8), rgba(0, 0, 163, 0.9)), 
              url('https://images.pexels.com/photos/841130/pexels-photo-841130.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1') no-repeat center center;
  background-size: cover;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  color: var(--cor-branco);
  margin-bottom: var(--espacamento-grande);
  
  &__conteudo {
    max-width: 800px;
    padding: 0 var(--espacamento-grande);
  }
  
  &__titulo {
    font-size: 4rem;
    font-weight: 700;
    margin-bottom: var(--espacamento-medio);
    animation: fadeInUp 0.8s ease;
  }
  
  &__subtitulo {
    font-size: var(--fonte-tamanho-grande);
    margin-bottom: var(--espacamento-grande);
    animation: fadeInUp 0.8s ease 0.2s both;
  }
  
  &__botao {
    animation: fadeInUp 0.8s ease 0.4s both;
  }
}

.secao {
  margin-bottom: var(--espacamento-extra-grande);
  
  &__cabecalho {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: var(--espacamento-grande);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande); 
    position: relative;
    
    &::after {
      content: '';
      position: absolute;
      left: 0;
      bottom: -10px;
      width: 50px;
      height: 3px;
      background-color: var(--cor-primaria);
    }
  }
  
  &__link {
    font-weight: 500;
  }

  &__conteudo {
    padding: var(--espacamento-medio) 0; 
    color: var(--cor-cinza-escuro); 
  }
}

.carregando {
  text-align: center;
  padding: var(--espacamento-grande) 0;
  color: var(--cor-cinza-escuro);
}

.produtos-carrossel {
  overflow: hidden;
  position: relative;
  
  &__container {
    display: flex;
    gap: var(--espacamento-medio);
    padding-bottom: var(--espacamento-pequeno);
    overflow-x: auto;
    scroll-behavior: smooth;
    -webkit-overflow-scrolling: touch;
    
    &::-webkit-scrollbar {
      height: 8px;
    }
    
    &::-webkit-scrollbar-track {
      background-color: var(--cor-cinza-claro);
      border-radius: 4px;
    }
    
    &::-webkit-scrollbar-thumb {
      background-color: var(--cor-primaria);
      border-radius: 4px;
    }
  }
  
  &__item {
    flex: 0 0 280px;
    transition: var(--transicao-padrao);
    
    @media (min-width: 576px) {
      flex: 0 0 300px;
    }
  }
}

.cta {
  background: linear-gradient(rgba(0, 0, 163, 0.9), rgba(0, 0, 163, 0.8)), 
              url('https://images.pexels.com/photos/703016/pexels-photo-703016.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1') no-repeat center;
  background-size: cover;
  padding: var(--espacamento-extra-grande) var(--espacamento-grande);
  border-radius: var(--borda-raio);
  
  &__conteudo {
    max-width: 600px;
    color: var(--cor-branco);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__texto {
    margin-bottom: var(--espacamento-grande);
    font-size: var(--fonte-tamanho-medio);
  }
  
  &__botao {
    background-color: var(--cor-secundaria);
    
    &:hover {
      background-color: var(--cor-secundaria-clara);
    }
  }
}

@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}
</style>