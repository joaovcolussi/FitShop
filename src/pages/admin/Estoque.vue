<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../services/api'

const produtos = ref<any[]>([])
const carregando = ref(true)
const erro = ref('')
const sucesso = ref(false)
const atualizandoEstoque = ref(false)

const carregarProdutos = async () => {
  carregando.value = true
  
  try {
    produtos.value = await api.getProdutos()
  } catch (error) {
    console.error('Erro ao carregar produtos', error)
    erro.value = 'Erro ao carregar a lista de produtos.'
  } finally {
    carregando.value = false
  }
}

const atualizarEstoque = async (produto: any) => {
  if (atualizandoEstoque.value) return
  
  atualizandoEstoque.value = true
  erro.value = ''
  
  try {
    await api.atualizarProduto(produto.id, { estoque: produto.estoque })
    
    sucesso.value = true
    setTimeout(() => {
      sucesso.value = false
    }, 3000)
  } catch (error) {
    console.error('Erro ao atualizar estoque', error)
    erro.value = 'Erro ao atualizar o estoque.'
    setTimeout(() => {
      erro.value = ''
    }, 3000)
  } finally {
    atualizandoEstoque.value = false
  }
}

const aumentarEstoque = (produto: any) => {
  produto.estoque++
  atualizarEstoque(produto)
}

const diminuirEstoque = (produto: any) => {
  if (produto.estoque > 0) {
    produto.estoque--
    atualizarEstoque(produto)
  }
}

const formatarPreco = (valor: number) => {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

onMounted(() => {
  carregarProdutos()
})
</script>

<template>
  <div class="admin-estoque">
    <div class="container">
      <header class="admin-estoque__cabecalho">
        <h1 class="admin-estoque__titulo">Controle de Estoque</h1>
        <router-link to="/admin" class="admin-estoque__voltar">
          Voltar ao painel
        </router-link>
      </header>
      
      <div v-if="sucesso" class="alerta alerta--sucesso">
        Estoque atualizado com sucesso!
      </div>
      
      <div v-if="erro" class="alerta alerta--erro">
        {{ erro }}
      </div>
      
      <div class="admin-estoque__conteudo">
        <div v-if="carregando" class="admin-estoque__carregando">
          <p>Carregando produtos...</p>
        </div>
        
        <div v-else-if="produtos.length === 0" class="admin-estoque__vazio">
          <p>Nenhum produto encontrado.</p>
        </div>
        
        <div v-else class="admin-estoque__tabela-container">
          <table class="tabela">
            <thead>
              <tr>
                <th>ID</th>
                <th>Produto</th>
                <th>Preço</th>
                <th>Estoque</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="produto in produtos" :key="produto.id">
                <td>{{ produto.id }}</td>
                <td>
                  <div class="tabela__produto">
                    <div class="tabela__imagem-container">
                      <img :src="produto.imagem" :alt="produto.nome" class="tabela__imagem" />
                    </div>
                    <div>
                      <div class="tabela__produto-nome">{{ produto.nome }}</div>
                      <div class="tabela__produto-categoria">{{ produto.categoria }}</div>
                    </div>
                  </div>
                </td>
                <td>{{ formatarPreco(produto.preco) }}</td>
                <td>
                  <span 
                    :class="{
                      'badge': true,
                      'badge--sucesso': produto.estoque > 10,
                      'badge--alerta': produto.estoque > 0 && produto.estoque <= 10,
                      'badge--erro': produto.estoque === 0
                    }"
                  >
                    {{ produto.estoque }}
                  </span>
                </td>
                <td>
                  <div class="tabela__estoque">
                    <button 
                      class="tabela__estoque-botao" 
                      @click="diminuirEstoque(produto)"
                      :disabled="produto.estoque <= 0 || atualizandoEstoque"
                    >
                      -
                    </button>
                    <span class="tabela__estoque-valor">{{ produto.estoque }}</span>
                    <button 
                      class="tabela__estoque-botao" 
                      @click="aumentarEstoque(produto)"
                      :disabled="atualizandoEstoque"
                    >
                      +
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.admin-estoque {
  &__cabecalho {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--espacamento-grande);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
  }
  
  &__voltar {
    display: inline-flex;
    align-items: center;
    padding: var(--espacamento-pequeno) var(--espacamento-medio);
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    transition: var(--transicao-padrao);
    
    &:hover {
      background-color: var(--cor-cinza-claro);
    }
  }
  
  &__conteudo {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
    margin-bottom: var(--espacamento-extra-grande);
  }
  
  &__carregando, &__vazio {
    text-align: center;
    padding: var(--espacamento-grande) 0;
    color: var(--cor-cinza-escuro);
  }
  
  &__tabela-container {
    overflow-x: auto;
  }
}

.tabela {
  width: 100%;
  border-collapse: collapse;
  
  th, td {
    padding: var(--espacamento-medio);
    text-align: left;
    border-bottom: 1px solid var(--cor-cinza-claro);
  }
  
  th {
    background-color: var(--cor-cinza-claro);
    font-weight: 600;
  }
  
  &__produto {
    display: flex;
    align-items: center;
    gap: var(--espacamento-medio);
  }
  
  &__imagem-container {
    width: 50px;
    height: 50px;
    border-radius: var(--borda-raio-pequeno);
    overflow: hidden;
    background-color: var(--cor-cinza-claro);
  }
  
  &__imagem {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  &__produto-nome {
    font-weight: 500;
  }
  
  &__produto-categoria {
    font-size: var(--fonte-tamanho-pequeno);
    color: var(--cor-cinza-escuro);
  }
  
  &__estoque {
    display: flex;
    align-items: center;
    gap: var(--espacamento-pequeno);
    
    &-botao {
      width: 30px;
      height: 30px;
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: var(--cor-cinza-claro);
      border: none;
      border-radius: var(--borda-raio-pequeno);
      font-weight: bold;
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
      width: 30px;
      text-align: center;
      font-weight: 500;
    }
  }
}
</style>