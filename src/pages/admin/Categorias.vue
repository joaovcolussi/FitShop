<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import api_whatsapp from '../../services/api'

const router = useRouter()
const categorias = ref<any[]>([])
const carregando = ref(true)
const erro = ref('')
const sucessoExclusao = ref(false)
const categoriaParaExcluir = ref<number | null>(null)
const confirmarExclusaoAberto = ref(false)

const carregarCategorias = async () => {
  carregando.value = true
  
  try {
    categorias.value = await api_whatsapp.getCategorias()
  } catch (error) {
    console.error('Erro ao carregar categorias', error)
    erro.value = 'Erro ao carregar a lista de categorias.'
  } finally {
    carregando.value = false
  }
}

const editarCategoria = (id: number) => {
  router.push(`/admin/categoria/${id}`)
}

const novaCategoria = () => {
  router.push('/admin/categoria')
}

const prepararExclusao = (id: number) => {
  categoriaParaExcluir.value = id
  confirmarExclusaoAberto.value = true
}

const cancelarExclusao = () => {
  categoriaParaExcluir.value = null
  confirmarExclusaoAberto.value = false
}

const confirmarExclusao = async () => {
  if (!categoriaParaExcluir.value) return
  
  try {
    await api_whatsapp.excluirCategoria(categoriaParaExcluir.value)
    categorias.value = categorias.value.filter(c => c.id !== categoriaParaExcluir.value)
    sucessoExclusao.value = true
    setTimeout(() => {
      sucessoExclusao.value = false
    }, 3000)
  } catch (error) {
    console.error('Erro ao excluir categoria', error)
    erro.value = 'Erro ao excluir a categoria.'
    setTimeout(() => {
      erro.value = ''
    }, 3000)
  } finally {
    cancelarExclusao()
  }
}

onMounted(() => {
  carregarCategorias()
})
</script>

<template>
  <div class="admin-categorias">
    <div class="container">
      <header class="admin-categorias__cabecalho">
        <h1 class="admin-categorias__titulo">Gerenciar Categorias</h1>
        <div class="admin-categorias__acoes">
          <router-link to="/admin" class="admin-categorias__voltar">
            Voltar ao painel
          </router-link>
          <button 
            class="botao botao--secundario" 
            @click="novaCategoria"
          >
            Nova Categoria
          </button>
        </div>
      </header>
      
      <div v-if="sucessoExclusao" class="alerta alerta--sucesso">
        Categoria excluída com sucesso!
      </div>
      
      <div v-if="erro" class="alerta alerta--erro">
        {{ erro }}
      </div>
      
      <div class="admin-categorias__conteudo">
        <div v-if="carregando" class="admin-categorias__carregando">
          <p>Carregando categorias...</p>
        </div>
        
        <div v-else-if="categorias.length === 0" class="admin-categorias__vazio">
          <p>Nenhuma categoria cadastrada.</p>
          <button class="botao" @click="novaCategoria">Adicionar categoria</button>
        </div>
        
        <div v-else class="admin-categorias__tabela-container">
          <table class="tabela">
            <thead>
              <tr>
                <th>ID</th>
                <th>Nome</th>
                <th>Slug</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="categoria in categorias" :key="categoria.id">
                <td>{{ categoria.id }}</td>
                <td>{{ categoria.nome }}</td>
                <td>{{ categoria.slug }}</td>
                <td>
                  <div class="tabela__acoes">
                    <button 
                      class="tabela__acao tabela__acao--editar" 
                      @click="editarCategoria(categoria.id)"
                      title="Editar"
                    >
                      ✏️
                    </button>
                    <button 
                      class="tabela__acao tabela__acao--excluir" 
                      @click="prepararExclusao(categoria.id)"
                      title="Excluir"
                    >
                      🗑️
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    
    <!-- Modal de confirmação de exclusão -->
    <div v-if="confirmarExclusaoAberto" class="modal">
      <div class="modal__conteudo">
        <h2 class="modal__titulo">Confirmar Exclusão</h2>
        <p class="modal__texto">
          Tem certeza que deseja excluir esta categoria? Esta ação não pode ser desfeita e pode afetar produtos.
        </p>
        <div class="modal__acoes">
          <button class="botao botao--contorno" @click="cancelarExclusao">
            Cancelar
          </button>
          <button class="botao botao--secundario" @click="confirmarExclusao">
            Sim, excluir
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.admin-categorias {
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