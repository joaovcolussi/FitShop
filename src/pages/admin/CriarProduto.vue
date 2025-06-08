<script setup lang="ts">
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import api_whatsapp from '../../services/api'

const router = useRouter()

const form = reactive({
  nome: '',
  descricao: '',
  preco: 0,
  precoPromocional: null,
  categoria: '',
  imagem: '',
  estoque: 0,
  promocao: false
})

const salvar = async () => {
  await api_whatsapp.criarProduto({
    nome: form.nome,
    descricao: form.descricao,
    preco: parseFloat(String(form.preco)),
    precoPromocional: form.promocao ? parseFloat(String(form.precoPromocional)) : null,
    categoria: form.categoria,
    imagem: form.imagem,
    estoque: form.estoque,
    promocao: form.promocao
  })
  router.push('/admin/produtos')
}
</script>

<template>
  <div class="criar-produto">
    <h1>Criar Produto</h1>

    <form @submit.prevent="salvar" class="form">
      <div class="coluna">
        <label>Nome do Produto *</label>
        <input v-model="form.nome" required />

        <label>Categoria *</label>
        <select v-model="form.categoria" required>
          <option disabled value="">Selecione uma categoria</option>
          <option>Suplementos</option>
          <option>Roupas</option>
          <option>Calçados</option>
          <option>Eletrônicos</option>
          <option>Acessórios</option>
        </select>

        <label>Preço *</label>
        <input type="number" v-model="form.preco" step="0.01" required />

        <div class="promocao">
          <input type="checkbox" v-model="form.promocao" id="promocaoCheck" />
          <label for="promocaoCheck">Este produto está em promoção?</label>
        </div>

        <div v-if="form.promocao">
          <label>Preço Promocional</label>
          <input type="number" v-model="form.precoPromocional" step="0.01" />
        </div>

        <label>Estoque *</label>
        <input type="number" v-model="form.estoque" required />
      </div>

      <div class="coluna">
        <label>Descrição *</label>
        <textarea v-model="form.descricao" rows="6" required />

        <label>Imagem</label>
        <img v-if="form.imagem" :src="form.imagem" class="preview" />
        <input v-model="form.imagem" placeholder="URL da imagem" />
      </div>

      <div class="botoes">
        <button type="button" @click="router.back()" class="cancelar">Cancelar</button>
        <button type="submit" class="salvar">Salvar Produto</button>
      </div>
    </form>
  </div>
</template>

<style scoped>
.criar-produto {
  max-width: 1000px;
  margin: 2rem auto;
  padding: 2rem;
  background: #fff;
  border-radius: 8px;
}

.form {
  display: flex;
  gap: 2rem;
  flex-wrap: wrap;
}

.coluna {
  flex: 1;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  min-width: 300px;
}

input, select, textarea {
  padding: 0.5rem;
  font-size: 1rem;
  width: 100%;
}

textarea {
  resize: vertical;
}

.preview {
  width: 150px;
  height: 150px;
  object-fit: cover;
  margin-bottom: 1rem;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.promocao {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.botoes {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 2rem;
  width: 100%;
}

button.cancelar {
  padding: 0.6rem 1.2rem;
  background: #eee;
  border: 1px solid #ccc;
  cursor: pointer;
  border-radius: 4px;
}

button.salvar {
  padding: 0.6rem 1.5rem;
  background: #ff5722;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

button.salvar:hover {
  background: #e64a19;
}
</style>
