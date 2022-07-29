<template>
  <v-container>
    <div class="home">
      <v-data-table
        :headers="headers"
        :items.sync="books"
        hide-default-footer
        class="elevation-1"
      >
        <template v-slot:item="row">
          <tr>
            <td>{{ row.item.title }}</td>
            <td>{{ row.item.author }}</td>
            <td>{{ row.item.pages }}</td>
            <td>{{ row.item.publisher }}</td>
            <td>
              <v-btn @click="borrowBook(row.item.id)">Pegar</v-btn>
            </td>
          </tr>
        </template>
      </v-data-table>

      <v-snackbar v-model="snackbar">
        <ul id="errors" class="no-bullets">
          <li v-for="item in errorMessage" :key="item.message">
            {{ item.message }}
          </li>
        </ul>

        <template v-slot:action="{ attrs }">
          <v-btn v-bind="attrs" @click="snackbar = false"> Close </v-btn>
        </template>
      </v-snackbar>
    </div>
    <Loading :active.sync="loading" :is-full-page="true"></Loading>
  </v-container>
</template>

<script>
import bookApi from "@/api/book";
import Loading from "vue-loading-overlay";
import "vue-loading-overlay/dist/vue-loading.css";

export default {
  name: "Home",

  data() {
    return {
      loading: false,
      snackbar: false,
      errorMessage: [],
      headers: [
        {
          text: "Titulo",
          align: "start",
          sortable: false,
          value: "title",
        },
        { text: "Autor", value: "author" },
        { text: "Nº Páginas", value: "pages" },
        { text: "Editora", value: "editor" },
        { text: "", value: "" },
      ],
      books: [],
    };
  },
  async mounted() {
    try {
      this.loading = true;
      await this.listBooks();
    } catch (err) {
      console.log(err);
      this.errorMessage.push({ message: err.message });
      this.snackbar = true;
    } finally {
      this.loading = false;
    }
  },
  methods: {
    async borrowBook(id) {
      let borrowResponse = [];
      let borrowedBook = false;
      this.loading = true;
      await bookApi
        .borrowBook(id)
        .then((data) => {
          borrowedBook = true;
        })
        .catch((error) => {
          borrowedBook = false;
          if (error.response) {
            borrowResponse = error.response.data;
          } else {
            console.log(error.message);
            borrowResponse.push({ message: error.message });
          }
        });

      if (borrowedBook) {
        await this.listBooks();
        this.snackbar = false;
      } else {
        this.errorMessage = borrowResponse;
        this.snackbar = true;
      }
      this.loading = false;
    },
    async listBooks() {
      const allBooks = await bookApi.listBooks();
      this.books = allBooks.data;
    },
  },
  components: {
    Loading,
  },
};
</script>

<style scoped>
.home {
  padding: 30px;
}
ul.no-bullets {
  list-style-type: none;
  margin: 0;
  padding: 0;
}
</style>
