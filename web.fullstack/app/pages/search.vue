<script setup lang="ts">
useHead({ title: 'Search posts' })

const route = useRoute()
const router = useRouter()

const query = ref(typeof route.query.q === 'string' ? route.query.q : '')
const submitted = computed(() => (typeof route.query.q === 'string' ? route.query.q : ''))

function onSearch() {
  const q = query.value.trim()
  router.replace({ query: q ? { q } : {} })
}
</script>

<template>
  <div class="min-h-dvh flex flex-col">
    <header class="sticky top-0 z-10 border-b border-lichen bg-fog/95 backdrop-blur">
      <div class="mx-auto flex max-w-5xl flex-col gap-3 px-4 py-4 sm:flex-row sm:items-center sm:gap-8 sm:px-6">
        <NuxtLink
          to="/search"
          class="font-display text-2xl font-extrabold tracking-tight text-ink"
        >
          Posts
        </NuxtLink>

        <form
          role="search"
          class="flex flex-1 items-stretch"
          @submit.prevent="onSearch"
        >
          <label for="search-input" class="sr-only">Search posts</label>
          <input
            id="search-input"
            v-model="query"
            type="search"
            name="q"
            autocomplete="off"
            placeholder="Search posts by title"
            class="h-12 min-w-0 flex-1 rounded-l-lg border-2 border-r-0 border-ink bg-field px-4 text-base text-ink placeholder:text-stone focus-visible:outline-offset-0"
          >
          <button
            type="submit"
            class="h-12 shrink-0 rounded-r-lg border-2 border-ink bg-moss px-5 font-semibold text-field transition-colors hover:bg-moss-deep active:bg-ink"
          >
            Search
          </button>
        </form>
      </div>
    </header>

    <main class="mx-auto w-full max-w-5xl flex-1 px-4 py-8 sm:px-6">
      <section
        aria-live="polite"
        class="flex min-h-80 flex-col justify-center rounded-lg border-2 border-dashed border-lichen px-6 py-10"
      >
       <p class="font-display text-xl font-semibold">
            Find a post
          </p>
          <p class="mt-2 max-w-prose text-stone">
            Type part of a title in the search box, then press Enter or select Search.
          </p>
      </section>
    </main>
  </div>
</template>
