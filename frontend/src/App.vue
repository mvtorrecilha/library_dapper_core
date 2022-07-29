<template>
  <v-app>    
    <v-app-bar app color="primary" dark>      
      <span>Biblioteca Library</span>
    </v-app-bar>

    <v-main>
      <router-view />      
    </v-main>
  </v-app>
</template>

<script>

export default {
  name: "App",

  components: {    
  },

  data: () => ({
    //
  }),
};
</script>
