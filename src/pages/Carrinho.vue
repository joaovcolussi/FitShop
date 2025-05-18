<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useCarrinhoStore } from '../stores/carrinho'
import CarrinhoItem from '../components/ui/CarrinhoItem.vue'
import api_whatsapp from '../services/api_whatsapp'

const router = useRouter()
const carrinhoStore = useCarrinhoStore()

const telefone = ref('')
const carregando = ref(false)
const erro = ref('')
const sucesso = ref(false)

const formatarPreco = (valor: number) => {
  return valor.toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  })
}

const valorTotal = computed(() => formatarPreco(carrinhoStore.valorTotal))
const carrinhoVazio = computed(() => carrinhoStore.itens.length === 0)

const continuarComprando = () => {
  router.push('/busca')
}

const limparCarrinho = () => {
  carrinhoStore.limparCarrinho()
}

const validarTelefone = (tel: string) => {
  // Validação simples de telefone brasileiro
  return /^\d{10,11}$/.test(tel.replace(/\D/g, ''))
}

const finalizarCompra = async () => {
  if (!telefone.value) {
    erro.value = 'Por favor, informe seu telefone para continuar'
    return
  }
  
  if (!validarTelefone(telefone.value)) {
    erro.value = 'Telefone inválido. Digite apenas números com DDD (11 dígitos)'
    return
  }
  
  if (carrinhoVazio.value) {
    erro.value = 'Seu carrinho está vazio'
    return
  }
  
  erro.value = ''
  carregando.value = true
  
  try {
    const resultado = await api_whatsapp.enviarPedidoWhatsApp(
      carrinhoStore.itens,
      carrinhoStore.valorTotal,
      telefone.value
    )
    
    window.open(resultado.url, '_blank')
    sucesso.value = true
    carrinhoStore.limparCarrinho()
  } catch (error) {
    console.error('Erro ao finalizar compra', error)
    erro.value = 'Erro ao processar pedido. Tente novamente.'
  } finally {
    carregando.value = false
  }
}
</script>

<template>
  <div class="carrinho">
    <div class="container">
      <h1 class="carrinho__titulo">Meu Carrinho</h1>
      
      <div class="carrinho__conteudo">
        <div v-if="carrinhoVazio" class="carrinho__vazio">
          <p class="carrinho__vazio-mensagem">Seu carrinho está vazio</p>
          <button class="botao" @click="continuarComprando">
            Continuar comprando
          </button>
        </div>
        
        <div v-else class="carrinho__grid">
          <div class="carrinho__lista">
            <div class="carrinho__cabecalho-lista">
              <h2 class="carrinho__subtitulo">Produtos</h2>
              <button class="carrinho__limpar" @click="limparCarrinho">
                Limpar carrinho
              </button>
            </div>
            
            <div class="carrinho__items">
              <CarrinhoItem 
                v-for="item in carrinhoStore.itens" 
                :key="item.id"
                :item="item"
              />
            </div>
            
            <div class="carrinho__botoes">
              <button class="botao botao--contorno" @click="continuarComprando">
                Continuar comprando
              </button>
            </div>
          </div>
          
          <div class="carrinho__resumo">
            <div class="resumo">
              <h2 class="resumo__titulo">Resumo do pedido</h2>
              
              <div class="resumo__linha">
                <span class="resumo__texto">Subtotal</span>
                <span class="resumo__valor">{{ valorTotal }}</span>
              </div>
              
              <div class="resumo__linha resumo__linha--total">
                <span class="resumo__texto resumo__texto--destaque">Total</span>
                <span class="resumo__valor resumo__valor--destaque">{{ valorTotal }}</span>
              </div>
              
              <div class="resumo__contato">
                <h3 class="resumo__subtitulo">Contato para envio do pedido</h3>
                
                <div class="campo">
                  <label for="telefone" class="campo__label">Telefone com DDD</label>
                  <input 
                    type="tel" 
                    id="telefone" 
                    v-model="telefone" 
                    placeholder="(11) 99999-9999"
                    class="campo__input" 
                  />
                </div>
                
                <button 
                  class="botao botao--secundario resumo__botao" 
                  @click="finalizarCompra"
                  :disabled="carregando"
                >
                  <span v-if="!carregando">Enviar pedido via WhatsApp</span>
                  <span v-else>Processando...</span>
                </button>
                
                <div v-if="erro" class="alerta alerta--erro">
                  {{ erro }}
                </div>
                
                <div v-if="sucesso" class="alerta alerta--sucesso">
                  Pedido enviado com sucesso via WhatsApp!
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style lang="scss" scoped>
.carrinho {
  &__titulo {
    font-size: var(--fonte-tamanho-grande);
    margin-bottom: var(--espacamento-grande);
  }
  
  &__conteudo {
    margin-bottom: var(--espacamento-extra-grande);
  }
  
  &__vazio {
    background-color: var(--cor-branco);
    padding: var(--espacamento-grande);
    border-radius: var(--borda-raio);
    text-align: center;
    
    &-mensagem {
      font-size: var(--fonte-tamanho-medio);
      margin-bottom: var(--espacamento-grande);
      color: var(--cor-cinza-escuro);
    }
  }
  
  &__grid {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--espacamento-grande);
    
    @media (min-width: 768px) {
      grid-template-columns: 2fr 1fr;
    }
  }
  
  &__lista {
    background-color: var(--cor-branco);
    border-radius: var(--borda-raio);
    padding: var(--espacamento-grande);
  }
  
  &__cabecalho-lista {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: var(--espacamento-grande);
  }
  
  &__subtitulo {
    font-size: var(--fonte-tamanho-medio);
  }
  
  &__limpar {
    background: none;
    border: none;
    color: var(--cor-erro);
    font-size: var(--fonte-tamanho-base);
    cursor: pointer;
    transition: var(--transicao-padrao);
    
    &:hover {
      text-decoration: underline;
    }
  }
  
  &__items {
    margin-bottom: var(--espacamento-grande);
  }
  
  &__botoes {
    display: flex;
    justify-content: flex-start;
  }
}

.resumo {
  background-color: var(--cor-branco);
  border-radius: var(--borda-raio);
  padding: var(--espacamento-grande);
  
  &__titulo {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-grande);
  }
  
  &__linha {
    display: flex;
    justify-content: space-between;
    margin-bottom: var(--espacamento-pequeno);
    
    &--total {
      border-top: 1px solid var(--cor-cinza-claro);
      margin-top: var(--espacamento-medio);
      padding-top: var(--espacamento-medio);
      margin-bottom: var(--espacamento-grande);
    }
  }
  
  &__texto {
    color: var(--cor-cinza-escuro);
    
    &--destaque {
      color: var(--cor-preto);
      font-weight: 700;
    }
  }
  
  &__valor {
    font-weight: 500;
    
    &--destaque {
      font-weight: 700;
      color: var(--cor-primaria);
      font-size: var(--fonte-tamanho-medio);
    }
  }
  
  &__contato {
    margin-top: var(--espacamento-grande);
  }
  
  &__subtitulo {
    font-size: var(--fonte-tamanho-base);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__botao {
    width: 100%;
    margin-top: var(--espacamento-medio);
  }
}
</style>