<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useCarrinhoStore } from '../stores/carrinho'
import api from '../services/api'

const route = useRoute()
const router = useRouter()
const carrinhoStore = useCarrinhoStore()

const produto = ref<any>(null)
const quantidade = ref(1)
const carregando = ref(true)
const erro = ref('')
const mensagemAdicionado = ref(false)

const produtoId = computed(() => {
  const id = Number(route.params.id)
  return isNaN(id) ? null : id
})

const precoFormatado = computed(() => {
  if (!produto.value) return ''
  return produto.value.preco.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
})

const carregarProduto = async () => {
  if (!produtoId.value) {
    erro.value = 'ID do produto inválido'
    carregando.value = false
    return
  }
  
  try {
    carregando.value = true
    produto.value = await api.getProduto(produtoId.value)
  } catch (error) {
    console.error('Erro ao carregar produto', error)
    erro.value = 'Erro ao carregar o produto. Tente novamente.'
  } finally {
    carregando.value = false
  }
}

const aumentarQuantidade = () => {
  if (quantidade.value < produto.value.estoque) {
    quantidade.value++
  }
}

const diminuirQuantidade = () => {
  if (quantidade.value > 1) {
    quantidade.value--
  }
}

const adicionarAoCarrinho = () => {
  if (!produto.value) return
  
  carrinhoStore.adicionarItem({
    id: produto.value.id,
    nome: produto.value.nome,
    preco: produto.value.preco,
    imagem: produto.value.imagem,
    quantidade: quantidade.value
  })
  
  mensagemAdicionado.value = true
  setTimeout(() => {
    mensagemAdicionado.value = false
  }, 3000)
}

const comprarAgora = () => {
  adicionarAoCarrinho()
  router.push('/carrinho')
}

onMounted(() => {
  carregarProduto()
})
</script>

<template>
  <div class="produto-detalhe">
    <div class="container">
      <div v-if="carregando" class="produto-detalhe__carregando">
        <p>Carregando detalhes do produto...</p>
      </div>
      
      <div v-else-if="erro" class="produto-detalhe__erro">
        <p>{{ erro }}</p>
        <router-link to="/busca" class="botao botao--contorno">
          Voltar para busca
        </router-link>
      </div>
      
      <div v-else-if="produto" class="produto-detalhe__conteudo">
        <div class="produto-detalhe__galeria">
          <div class="produto-detalhe__imagem-container">
            <img :src="produto.imagem" :alt="produto.nome" class="produto-detalhe__imagem" />
          </div>
        </div>
        
        <div class="produto-detalhe__info">
          <div class="produto-detalhe__categoria">{{ produto.categoria }}</div>
          <h1 class="produto-detalhe__titulo">{{ produto.nome }}</h1>
          
          <div class="produto-detalhe__preco">{{ precoFormatado }}</div>
          
          <div class="produto-detalhe__descricao">
            <p>{{ produto.descricao }}</p>
          </div>
          
          <div class="produto-detalhe__estoque">
            <span v-if="produto.estoque > 0" class="badge badge--sucesso">Em estoque</span>
            <span v-else class="badge badge--erro">Fora de estoque</span>
          </div>
          
          <div class="produto-detalhe__compra">
            <div class="produto-detalhe__quantidade">
              <button 
                class="produto-detalhe__quantidade-botao" 
                @click="diminuirQuantidade"
                :disabled="quantidade <= 1"
              >
                -
              </button>
              <span class="produto-detalhe__quantidade-valor">{{ quantidade }}</span>
              <button 
                class="produto-detalhe__quantidade-botao" 
                @click="aumentarQuantidade"
                :disabled="quantidade >= produto.estoque"
              >
                +
              </button>
            </div>
            
            <div class="produto-detalhe__acoes">
              <button 
                class="botao produto-detalhe__botao-carrinho" 
                @click="adicionarAoCarrinho"
                :disabled="produto.estoque === 0"
              >
                Adicionar ao carrinho
              </button>
              <button 
                class="botao botao--secundario produto-detalhe__botao-comprar" 
                @click="comprarAgora"
                :disabled="produto.estoque === 0"
              >
                Comprar agora
              </button>
            </div>
          </div>
          
          <div v-if="mensagemAdicionado" class="alerta alerta--sucesso produto-detalhe__alerta">
            Produto adicionado ao carrinho!
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.produto-detalhe {
  &__carregando, &__erro {
    padding: var(--espacamento-grande);
    text-align: center;
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    margin-bottom: var(--espacamento-grande);
  }
  
  &__conteudo {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--espacamento-grande);
    
    @media (min-width: 768px) {
      grid-template-columns: 1fr 1fr;
    }
  }
  
  &__galeria {
    position: sticky;
    top: 100px;
  }
  
  &__imagem-container {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    overflow: hidden;
    height: 400px;
    display: flex;
    align-items: center;
    justify-content: center;
    
    @media (min-width: 992px) {
      height: 500px;
    }
  }
  
  &__imagem {
    max-width: 100%;
    max-height: 100%;
    object-fit: contain;
  }
  
  &__info {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
  }
  
  &__categoria {
    color: var(--cor-cinza-escuro);
    margin-bottom: var(--espacamento-pequeno);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-extra-grande);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__preco {
    font-size: 2rem;
    font-weight: 700;
    color: var(--cor-primaria);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__descricao {
    color: var(--cor-cinza-escuro);
    margin-bottom: var(--espacamento-grande);
    line-height: 1.6;
  }
  
  &__estoque {
    margin-bottom: var(--espacamento-medio);
  }
  
  &__compra {
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-medio);
    margin-bottom: var(--espacamento-medio);
    
    @media (min-width: 480px) {
      flex-direction: row;
      align-items: center;
    }
  }
  
  &__quantidade {
    display: flex;
    align-items: center;
    background-color: var(--cor-cinza-claro);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-pequeno);
    
    &-botao {
      width: 36px;
      height: 36px;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: var(--cor-branco);
      border: none;
      border-radius: var(--borda-raio-pequeno);
      font-weight: bold;
      font-size: 1.2rem;
      cursor: pointer;
      transition: var(--transicao-padrao);
      
      &:hover:not(:disabled) {
        background-color: var(--cor-primaria);
        color: var(--cor-branco);
      }
      
      &:disabled {
        opacity: 0.5;
        cursor: not-allowed;
      }
    }
    
    &-valor {
      width: 40px;
      text-align: center;
      font-weight: 500;
      font-size: 1.1rem;
    }
  }
  
  &__acoes {
    display: flex;
    flex-wrap: wrap;
    gap: var(--espacamento-pequeno);
    flex: 1;
  }
  
  &__botao-carrinho, &__botao-comprar {
    flex: 1;
    
    &:disabled {
      background-color: var(--cor-cinza);
      cursor: not-allowed;
      
      &:hover {
        background-color: var(--cor-cinza);
      }
    }
  }
  
  &__alerta {
    margin-top: var(--espacamento-medio);
  }
}
</style>