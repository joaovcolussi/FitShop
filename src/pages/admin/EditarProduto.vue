<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import api from '../../services/api'

const route = useRoute()
const router = useRouter()

const produto = ref({
  id: 0,
  nome: '',
  descricao: '',
  preco: 0,
  categoria: '',
  imagem: '',
  estoque: 0
})

const precoFormatado = computed({
  get: () => produto.value.preco.toString(),
  set: (valor) => {
    const numeroLimpo = valor.replace(/[^\d.,]/g, '')
    const numeroFormatado = numeroLimpo.replace(',', '.')
    produto.value.preco = parseFloat(numeroFormatado) || 0
  }
})

const estoqueFormatado = computed({
  get: () => produto.value.estoque.toString(),
  set: (valor) => {
    produto.value.estoque = parseInt(valor) || 0
  }
})

const categorias = ref<any[]>([])
const imagemArquivo = ref<File | null>(null)
const imagemPreview = ref('')
const carregando = ref(false)
const salvando = ref(false)
const erro = ref('')
const sucesso = ref(false)
const modoEdicao = computed(() => !!route.params.id)

const carregarDados = async () => {
  carregando.value = true
  
  try {
    // Carrega categorias
    categorias.value = await api.getCategorias()
    
    // Carrega produto se for edição
    if (modoEdicao.value) {
      const id = Number(route.params.id)
      if (!isNaN(id)) {
        const produtoCarregado = await api.getProduto(id)
        produto.value = { ...produtoCarregado }
        imagemPreview.value = produtoCarregado.imagem
      }
    }
  } catch (error) {
    console.error('Erro ao carregar dados', error)
    erro.value = 'Erro ao carregar dados. Tente novamente.'
  } finally {
    carregando.value = false
  }
}

const handleImagemChange = (event: Event) => {
  const input = event.target as HTMLInputElement
  const files = input.files
  
  if (files && files.length > 0) {
    imagemArquivo.value = files[0]
    
    // Cria uma URL temporária para exibir a imagem
    imagemPreview.value = URL.createObjectURL(files[0])
  }
}

const salvarProduto = async () => {
  if (!produto.value.nome || !produto.value.descricao || !produto.value.categoria) {
    erro.value = 'Por favor, preencha todos os campos obrigatórios.'
    return
  }
  
  salvando.value = true
  erro.value = ''
  
  try {
    // Em uma implementação real, aqui faria upload da imagem para um servidor
    // e obteria a URL da imagem. Como isso é um protótipo, estamos simulando:
    
    // Se temos um arquivo de imagem novo, simular upload
    if (imagemArquivo.value) {
      // Em um backend real, faríamos o upload real e obteríamos uma URL
      // Para o protótipo, usamos uma URL de placeholder ou mantemos a existente
      
      // Simula o processamento de upload - para o protótipo mantemos a URL existente
      // Em caso de upload real, atualizaríamos product.imagem com a nova URL
    }
    
    // Salva o produto
    if (modoEdicao.value) {
      await api.atualizarProduto(produto.value.id, produto.value)
      sucesso.value = true
      setTimeout(() => {
        router.push('/admin/produtos')
      }, 1500)
    } else {
      await api.criarProduto(produto.value)
      sucesso.value = true
      setTimeout(() => {
        router.push('/admin/produtos')
      }, 1500)
    }
  } catch (error) {
    console.error('Erro ao salvar produto', error)
    erro.value = 'Erro ao salvar o produto. Tente novamente.'
  } finally {
    salvando.value = false
  }
}

const cancelar = () => {
  router.push('/admin/produtos')
}

onMounted(() => {
  carregarDados()
})
</script>

<template>
  <div class="editar-produto">
    <div class="container">
      <header class="editar-produto__cabecalho">
        <h1 class="editar-produto__titulo">
          {{ modoEdicao ? 'Editar Produto' : 'Novo Produto' }}
        </h1>
        <button class="editar-produto__voltar" @click="cancelar">
          Voltar
        </button>
      </header>
      
      <div v-if="carregando" class="editar-produto__carregando">
        <p>Carregando dados...</p>
      </div>
      
      <div v-else class="editar-produto__conteudo">
        <form @submit.prevent="salvarProduto" class="editar-produto__form">
          <div class="editar-produto__grid">
            <div class="editar-produto__coluna">
              <div class="campo">
                <label for="nome" class="campo__label">Nome do Produto *</label>
                <input 
                  type="text" 
                  id="nome" 
                  v-model="produto.nome" 
                  class="campo__input" 
                  placeholder="Digite o nome do produto"
                  required
                />
              </div>
              
              <div class="campo">
                <label for="categoria" class="campo__label">Categoria *</label>
                <select 
                  id="categoria" 
                  v-model="produto.categoria" 
                  class="campo__input"
                  required
                >
                  <option value="" disabled>Selecione uma categoria</option>
                  <option 
                    v-for="categoria in categorias" 
                    :key="categoria.id"
                    :value="categoria.nome"
                  >
                    {{ categoria.nome }}
                  </option>
                </select>
              </div>
              
              <div class="campo">
                <label for="preco" class="campo__label">Preço *</label>
                <input 
                  type="text" 
                  id="preco" 
                  v-model="precoFormatado" 
                  class="campo__input" 
                  placeholder="0,00"
                  required
                />
              </div>
              
              <div class="campo">
                <label for="estoque" class="campo__label">Estoque *</label>
                <input 
                  type="number" 
                  id="estoque" 
                  v-model="estoqueFormatado" 
                  class="campo__input" 
                  min="0"
                  required
                />
              </div>
            </div>
            
            <div class="editar-produto__coluna">
              <div class="campo">
                <label for="descricao" class="campo__label">Descrição *</label>
                <textarea 
                  id="descricao" 
                  v-model="produto.descricao" 
                  class="campo__input campo__input--textarea" 
                  placeholder="Descreva o produto"
                  rows="5"
                  required
                ></textarea>
              </div>
              
              <div class="campo">
                <label class="campo__label">Imagem</label>
                <div class="editar-produto__imagem-container">
                  <div 
                    class="editar-produto__imagem-preview" 
                    :class="{ 'editar-produto__imagem-preview--vazio': !imagemPreview }"
                  >
                    <img 
                      v-if="imagemPreview" 
                      :src="imagemPreview" 
                      alt="Preview" 
                      class="editar-produto__imagem"
                    />
                    <div v-else class="editar-produto__imagem-placeholder">
                      Sem imagem
                    </div>
                  </div>
                  
                  <div class="editar-produto__imagem-acoes">
                    <label for="imagem-upload" class="botao botao--contorno">
                      {{ imagemPreview ? 'Alterar imagem' : 'Selecionar imagem' }}
                    </label>
                    <input 
                      type="file" 
                      id="imagem-upload" 
                      @change="handleImagemChange" 
                      accept="image/*"
                      class="editar-produto__imagem-input"
                    />
                    
                    <div v-if="imagemPreview" class="editar-produto__imagem-alternativa">
                      <p>Ou informe uma URL:</p>
                      <input 
                        type="text" 
                        v-model="produto.imagem" 
                        class="campo__input" 
                        placeholder="https://exemplo.com/imagem.jpg"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <div v-if="erro" class="alerta alerta--erro">
            {{ erro }}
          </div>
          
          <div v-if="sucesso" class="alerta alerta--sucesso">
            Produto salvo com sucesso! Redirecionando...
          </div>
          
          <div class="editar-produto__acoes">
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
              <span v-if="!salvando">Salvar Produto</span>
              <span v-else>Salvando...</span>
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.editar-produto {
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
  }
  
  &__grid {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--espacamento-grande);
    margin-bottom: var(--espacamento-grande);
    
    @media (min-width: 768px) {
      grid-template-columns: 1fr 1fr;
    }
  }
  
  &__imagem-container {
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-medio);
    
    @media (min-width: 576px) {
      flex-direction: row;
      align-items: flex-start;
    }
  }
  
  &__imagem-preview {
    width: 200px;
    height: 200px;
    border-radius: var(--borda-raio);
    overflow: hidden;
    background-color: var(--cor-cinza-claro);
    
    &--vazio {
      display: flex;
      align-items: center;
      justify-content: center;
      border: 2px dashed var(--cor-cinza);
    }
  }
  
  &__imagem {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }
  
  &__imagem-placeholder {
    color: var(--cor-cinza-escuro);
    font-size: var(--fonte-tamanho-pequeno);
  }
  
  &__imagem-acoes {
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-medio);
  }
  
  &__imagem-input {
    display: none;
  }
  
  &__imagem-alternativa {
    margin-top: var(--espacamento-pequeno);
    
    p {
      margin-bottom: var(--espacamento-pequeno);
      font-size: var(--fonte-tamanho-pequeno);
      color: var(--cor-cinza-escuro);
    }
  }
  
  &__acoes {
    display: flex;
    justify-content: flex-end;
    gap: var(--espacamento-medio);
    margin-top: var(--espacamento-grande);
  }
}

.campo {
  &__input {
    &--textarea {
      resize: vertical;
      min-height: 100px;
    }
  }
}
</style>