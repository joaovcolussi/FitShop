<template>
  <footer class="rodape">
    <div class="container">
      <div class="rodape__conteudo">
        <div class="rodape__coluna">
          <h3 class="rodape__titulo">FitShop</h3>
          <p class="rodape__descricao">
            Sua loja online de produtos com as melhores ofertas do mercado.
          </p>
        </div>
        
        <div class="rodape__coluna">
          <h4 class="rodape__titulo">Links Úteis</h4>
          <ul class="rodape__links">
            <li><router-link to="/" class="rodape__link">Home</router-link></li>
            <li><router-link to="/busca" class="rodape__link">Produtos</router-link></li>
            <li><router-link to="/carrinho" class="rodape__link">Carrinho</router-link></li>
          </ul>
        </div>
        
        <div class="rodape__coluna">
          <h4 class="rodape__titulo">Categorias</h4>
          <ul class="rodape__links">
            <li><router-link to="/busca?categoria=suplementos" class="rodape__link">Suplementos</router-link></li>
            <li><router-link to="/busca?categoria=acessorios" class="rodape__link">Acessórios</router-link></li>
            <li><router-link to="/busca?categoria=roupas" class="rodape__link">Roupas Fitness</router-link></li>
          </ul>
        </div>
        
        <div class="rodape__coluna">
          <h4 class="rodape__titulo">Contato</h4>
          <address class="rodape__contato">
            <p>Email: contato@fitshop.com.br</p>
          </address>
          <div class="rodape__redes">
            <a href="#" class="rodape__rede">Instagram</a>
            <a href="#" class="rodape__rede">Facebook</a>
          </div>
        </div>
      </div>
      
      <div class="rodape__direitos">
        <p>&copy; 2025 FitShop - Todos os direitos reservados</p>
      </div>
    </div>
  </footer>
</template>

<style lang="scss" scoped>
.rodape {
  background-color: var(--cor-primaria-escura);
  color: var(--cor-branco);
  padding: var(--espacamento-grande) 0 var(--espacamento-medio);
  margin-top: var(--espacamento-extra-grande);
  
  &__conteudo {
    display: grid;
    grid-template-columns: 1fr;
    gap: var(--espacamento-grande);
    
    @media (min-width: 576px) {
      grid-template-columns: repeat(2, 1fr);
    }
    
    @media (min-width: 992px) {
      grid-template-columns: repeat(4, 1fr);
    }
  }
  
  &__titulo {
    font-size: var(--fonte-tamanho-medio);
    margin-bottom: var(--espacamento-medio);
    position: relative;
    
    &::after {
      content: '';
      position: absolute;
      left: 0;
      bottom: -8px;
      width: 40px;
      height: 2px;
      background-color: var(--cor-secundaria);
    }
  }
  
  &__descricao {
    line-height: 1.5;
  }
  
  &__links {
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-pequeno);
  }
  
  &__link {
    color: var(--cor-cinza);
    transition: var(--transicao-padrao);
    
    &:hover {
      color: var(--cor-branco);
      padding-left: var(--espacamento-pequeno);
    }
  }
  
  &__contato {
    font-style: normal;
    display: flex;
    flex-direction: column;
    gap: var(--espacamento-pequeno);
    margin-bottom: var(--espacamento-medio);
  }
  
  &__redes {
    display: flex;
    gap: var(--espacamento-medio);
  }
  
  &__rede {
    color: var(--cor-cinza);
    transition: var(--transicao-padrao);
    
    &:hover {
      color: var(--cor-branco);
    }
  }
  
  &__direitos {
    margin-top: var(--espacamento-grande);
    padding-top: var(--espacamento-medio);
    text-align: center;
    border-top: 1px solid rgba(255, 255, 255, 0.1);
    font-size: var(--fonte-tamanho-pequeno);
    color: var(--cor-cinza);
  }
}
</style>