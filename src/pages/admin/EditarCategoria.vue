<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api_whatsapp from '../../services/api'

const route = useRoute()
const router = useRouter()

const categoria = ref({
  id: 0,
  nome: '',
  slug: ''
})

const carregando = ref(false)
const salvando = ref(false)
const erro = ref('')
const sucesso = ref(false)
const modoEdicao = computed(() => !!route.params.id)

const gerarSlug = (texto: string) => {
  return texto
    .toLowerCase()
    .normalize('NFD')
    .replace(/[\u0300-\u036f]/g, '')
    .replace(/[^\w\s-]/g, '')
    .replace(/\s+/g, '-')
    .replace(/--+/g, '-')
    .trim()
}

const atualizarSlug = () => {
  if (!modoEdicao.value || !categoria.value.slug) {
    categoria.value.slug = gerarSlug(categoria.value.nome)
  }
}

const carregarCategoria = async () => {
  if (!modoEdicao.value) return
  
  carregando.value = true
  
  try {
    const id = Number(route.params.id)
    if (!isNaN(id)) {
      const categorias = await api_whatsapp.getCategorias()
      const categoriaEncontrada = categorias.find((c: any) => c.id === id)
      
      if (categoriaEncontrada) {
        categoria.value = { ...categoriaEncontrada }
      } else {
        throw new Error('Categoria não encontrada')
      }
    }
  } catch (error) {
    console.error('Erro ao carregar categoria', error)
    erro.value = 'Erro ao carregar categoria. Tente novamente.'
  } finally {
    carregando.value = false
  }
}

const salvarCategoria = async () => {
  if (!categoria.value.nome) {
    erro.value = 'O nome da categoria é obrigatório.'
    return
  }
  
  if (!categoria.value.slug) {
    atualizarSlug()
  }
  
  salvando.value = true
  erro.value = ''
  
  try {
    if (modoEdicao.value) {
      await api_whatsapp.atualizarCategoria(categoria.value.id, categoria.value)
    } else {
      await api_whatsapp.criarCategoria(categoria.value)
    }
    
    sucesso.value = true
    setTimeout(() => {
      router.push('/admin/categorias')
    }, 1500)
  } catch (error) {
    console.error('Erro ao salvar categoria', error)
    erro.value = 'Erro ao salvar categoria. Tente novamente.'
  } finally {
    salvando.value = false
  }
}

const cancelar = () => {
  router.push('/admin/categorias')
}

onMounted(() => {
  carregarCategoria()
})
</script>

<template>
  <div class="editar-categoria">
    <div class="container">
      <header class="editar-categoria__cabecalho">
        <h1 class="editar-categoria__titulo">
          {{ modoEdicao ? 'Editar Categoria' : 'Nova Categoria' }}
        </h1>
        <button class="editar-categoria__voltar" @click="cancelar">
          Voltar
        </button>
      </header>
      
      <div v-if="carregando" class="editar-categoria__carregando">
        <p>Carregando dados...</p>
      </div>
      
      <div v-else class="editar-categoria__conteudo">
        <form @submit.prevent="salvarCategoria" class="editar-categoria__form">
          <div class="campo">
            <label for="nome" class="campo__label">Nome da Categoria *</label>
            <input 
              type="text" 
              id="nome" 
              v-model="categoria.nome" 
              @input="atualizarSlug"
              class="campo__input" 
              placeholder="Digite o nome da categoria"
              required
            />
          </div>
          
          <div class="campo">
            <label for="slug" class="campo__label">
              Slug
              <span class="campo__info">(identificador único usado nas URLs)</span>
            </label>
            <input 
              type="text" 
              id="slug" 
              v-model="categoria.slug" 
              class="campo__input" 
              placeholder="identificador-da-categoria"
            />
          </div>
          
          <div v-if="erro" class="alerta alerta--erro">
            {{ erro }}
          </div>
          
          <div v-if="sucesso" class="alerta alerta--sucesso">
            Categoria salva com sucesso! Redirecionando...
          </div>
          
          <div class="editar-categoria__acoes">
            <button 
              type="button"
              class="botao botao--contorno"
              @click="cancelar"
            >
              Cancelar
            </button>
            <button 
              type="submit"
              class="botao botao--secundario"
              :disabled="salvando"
            >
              <span v-if="!salvando">Salvar Categoria</span>
              <span v-else>Salvando...</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.editar-categoria {
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
    border: none;
    border-radius: var(--borda-raio);
    transition: var(--transicao-padrao);
    cursor: pointer;
    
    &:hover {
      background-color: var(--cor-cinza-claro);
    }
  }
  
  &__carregando {
    background-color: var(--cor-branco);
    padding: var(--espacamento-grande);
    border-radius: var(--borda-raio);
    text-align: center;
  }
  
  &__conteudo {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
    margin-bottom: var(--espacamento-extra-grande);
    max-width: 600px;
    margin-left: auto;
    margin-right: auto;
  }
  
  &__acoes {
    display: flex;
    justify-content: flex-end;
    gap: var(--espacamento-medio);
    margin-top: var(--espacamento-grande);
  }
}

.campo {
  &__info {
    font-size: var(--fonte-tamanho-pequeno);
    color: var(--cor-cinza-escuro);
    font-weight: normal;
    margin-left: var(--espacamento-pequeno);
  }
}
</style>