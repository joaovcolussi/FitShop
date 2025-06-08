<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api_whatsapp from '../../services/api'

const router = useRouter()
const produtos = ref<any[]>([])
const carregando = ref(true)
const erro = ref('')
const sucessoExclusao = ref(false)
const produtoParaExcluir = ref<number | null>(null)
const confirmarExclusaoAberto = ref(false)

const carregarProdutos = async () => {
  carregando.value = true
  try {
    produtos.value = await api_whatsapp.getProdutos()
  } catch (error) {
    console.error('Erro ao carregar produtos', error)
    erro.value = 'Erro ao carregar a lista de produtos.'
  } finally {
    carregando.value = false
  }
}

const editarProduto = (id: number) => {
  router.push(`/admin/produto/${id}`)
}

const novoProduto = () => {
  router.push('/admin/produto/novo')
}

const prepararExclusao = (id: number) => {
  produtoParaExcluir.value = id
  confirmarExclusaoAberto.value = true
}

const cancelarExclusao = () => {
  produtoParaExcluir.value = null
  confirmarExclusaoAberto.value = false
}

const confirmarExclusao = async () => {
  if (!produtoParaExcluir.value) return

  try {
    await api_whatsapp.excluirProduto(produtoParaExcluir.value)
    produtos.value = produtos.value.filter(p => p.id !== produtoParaExcluir.value)
    sucessoExclusao.value = true
    setTimeout(() => {
      sucessoExclusao.value = false
    }, 3000)
  } catch (error) {
    console.error('Erro ao excluir produto', error)
    erro.value = 'Erro ao excluir o produto.'
    setTimeout(() => {
      erro.value = ''
    }, 3000)
  } finally {
    cancelarExclusao()
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
  <div class="admin-produtos">
    <div class="container">
      <header class="admin-produtos__cabecalho">
        <h1 class="admin-produtos__titulo">Gerenciar Produtos</h1>
        <div class="admin-produtos__acoes">
          <router-link to="/admin" class="admin-produtos__voltar">
            Voltar ao painel
          </router-link>
          <button class="botao botao--secundario" @click="novoProduto">
            Novo Produto
          </button>
        </div>
      </header>

      <div v-if="sucessoExclusao" class="alerta alerta--sucesso">
        Produto excluído com sucesso!
      </div>

      <div v-if="erro" class="alerta alerta--erro">
        {{ erro }}
      </div>

      <div class="admin-produtos__conteudo">
        <div v-if="carregando" class="admin-produtos__carregando">
          <p>Carregando produtos...</p>
        </div>

        <div v-else-if="produtos.length === 0" class="admin-produtos__vazio">
          <p>Nenhum produto cadastrado.</p>
          <button class="botao" @click="novoProduto">Adicionar produto</button>
        </div>

        <div v-else class="admin-produtos__tabela-container">
          <table class="tabela">
            <thead>
              <tr>
                <th>ID</th>
                <th>Imagem</th>
                <th>Nome</th>
                <th>Categoria</th>
                <th>Preço</th>
                <th>Estoque</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="produto in produtos" :key="produto.id">
                <td>{{ produto.id }}</td>
                <td>
                  <div class="tabela__imagem-container">
                    <img :src="produto.imagem" :alt="produto.nome" class="tabela__imagem" />
                  </div>
                </td>
                <td>{{ produto.nome }}</td>
                <td>{{ produto.categoria }}</td>
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
                  <div class="tabela__acoes">
                    <button class="tabela__acao tabela__acao--editar" @click="editarProduto(produto.id)" title="Editar">✏️</button>
                    <button class="tabela__acao tabela__acao--excluir" @click="prepararExclusao(produto.id)" title="Excluir">🗑️</button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>

    <div v-if="confirmarExclusaoAberto" class="modal">
      <div class="modal__conteudo">
        <h2 class="modal__titulo">Confirmar Exclusão</h2>
        <p class="modal__texto">
          Tem certeza que deseja excluir este produto? Esta ação não pode ser desfeita.
        </p>
        <div class="modal__acoes">
          <button class="botao botao--contorno" @click="cancelarExclusao">Cancelar</button>
          <button class="botao botao--secundario" @click="confirmarExclusao">Sim, excluir</button>
        </div>
      </div>
    </div>
  </div>
</template>


<style lang="scss" scoped>
.admin-produtos {
  &__cabecalho {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--espacamento-grande);
    flex-wrap: wrap;
    gap: var(--espacamento-medio);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
  }
  
  &__acoes {
    display: flex;
    gap: var(--espacamento-medio);
    align-items: center;
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
  
  &__vazio {
    button {
      margin-top: var(--espacamento-medio);
    }
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
  
  &__imagem-container {
    width: 60px;
    height: 60px;
    border-radius: var(--borda-raio-pequeno);
    overflow: hidden;
    background-color: var(--cor-cinza-claro);
  }
  
  &__imagem {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  &__acoes {
    display: flex;
    gap: var(--espacamento-pequeno);
  }
  
  &__acao {
    background: none;
    border: none;
    cursor: pointer;
    padding: var(--espacamento-pequeno);
    border-radius: var(--borda-raio-pequeno);
    transition: var(--transicao-padrao);
    
    &--editar:hover {
      background-color: rgba(0, 0, 163, 0.1);
    }
    
    &--excluir:hover {
      background-color: rgba(244, 67, 54, 0.1);
    }
  }
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1001;
  
  &__conteudo {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
    width: 100%;
    max-width: 500px;
    box-shadow: var(--sombra-grande);
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__texto {
    margin-bottom: var(--espacamento-grande);
    color: var(--cor-cinza-escuro);
  }
  
  &__acoes {
    display: flex;
    justify-content: flex-end;
    gap: var(--espacamento-medio);
  }
}
</style>