# FunnyToast — Catálogo de Plugins

Índice oficial de plugins para o [FunnyToast](Sem nada por agr colocar o link depois), o sistema de notificações e widgets para Windows.

Este repositório contém **apenas o catálogo** (`plugins.json`). Os binários dos plugins vivem nos Releases dos repositórios de cada plugin — este índice só aponta para eles.

## Como instalar plugins

Com o FunnyToast instalado, na linha de comandos:

```
FunnyToast.exe plugin search              # lista tudo o que há no catálogo
FunnyToast.exe plugin search rede         # filtra por termo
FunnyToast.exe plugin install <id>        # instala pelo id do catálogo
FunnyToast.exe plugin list                # lista o que tens instalado
FunnyToast.exe plugin remove <id>         # remove um plugin
```

Também podes instalar diretamente de um URL ou ficheiro, sem passar pelo catálogo:

```
FunnyToast.exe plugin install https://github.com/.../MeuPlugin.dll
FunnyToast.exe plugin install C:\caminho\MeuPlugin.zip
```

## ⚠️ Aviso de segurança

Plugins do FunnyToast são **código executável com acesso total ao teu sistema**. Não há sandbox. Um plugin pode ler ficheiros, aceder à rede e fazer tudo o que a tua conta de utilizador permite.

Instala apenas plugins de fontes em que confias. Este catálogo é mantido pela comunidade e **não há revisão de segurança automática** das entradas. Antes de instalar, verifica o repositório de origem (campo `homepageUrl`) e, idealmente, o código-fonte.

## Submeter um plugin ao catálogo

1. Publica o `.dll` (ou um `.zip` com o `.dll` + dependências) num **Release** do repositório do teu plugin.
2. Faz fork deste repositório.
3. Adiciona uma entrada ao `plugins.json` (ver formato abaixo).
4. Abre um Pull Request.

### Formato de uma entrada

```json
{
  "id": "FunnyToast.Plugin.Exemplo",
  "displayName": "Nome Visível",
  "version": "1.0.0",
  "author": "O teu nome",
  "description": "Uma frase a descrever o que o plugin faz.",
  "downloadUrl": "https://github.com/USER/repo/releases/download/v1.0.0/Plugin.dll",
  "homepageUrl": "https://github.com/USER/repo",
  "tags": ["categoria", "outra-tag"]
}
```

| Campo | Obrigatório | Descrição |
|-------|:-:|-----------|
| `id` | sim | Identificador único. Deve corresponder ao nome do ficheiro `.dll` sem extensão. |
| `displayName` | sim | Nome mostrado ao utilizador. |
| `version` | sim | Versão semântica (ex: `1.2.0`). |
| `author` | não | Autor do plugin. |
| `description` | não | Descrição curta (uma frase). |
| `downloadUrl` | sim | URL direto para o `.dll` ou `.zip` (normalmente um release asset do GitHub). |
| `homepageUrl` | não | Página do projeto, para o utilizador verificar a origem. |
| `tags` | não | Lista de tags para pesquisa. |

### Regras

- O `downloadUrl` deve ser um **link direto** (release asset), não a página do release.
- Usa URLs `https://`.
- O `id` tem de ser único no catálogo.
- Mantém a `description` numa só frase.

## Estrutura do catálogo

```json
{
  "schemaVersion": "1.0",
  "plugins": [ /* array de entradas */ ]
}
```

O `schemaVersion` permite evoluir o formato sem partir clientes antigos.

## Catálogo (raw)

O FunnyToast consome este URL:

```
https://raw.githubusercontent.com/SEU_USER/funnytoast-plugins/main/plugins.json
```

## Licença

O catálogo (este `plugins.json` e documentação) é disponibilizado sob MIT. Cada plugin listado tem a sua própria licença — consulta o repositório de origem.
