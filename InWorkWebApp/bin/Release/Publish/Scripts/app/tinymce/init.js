tinymce.init({
    selector: '#richtextarea',
    plugins: 'print preview importcss tinydrive searchreplace autolink autosave save directionality visualblocks visualchars fullscreen image link media  template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons',
    mobile: {
        plugins: 'print preview importcss tinydrive searchreplace autolink autosave save directionality visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount textpattern noneditable help charmap quickbars emoticons'
    },
    menubar: 'edit insert format table',
    toolbar: 'undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor casechange removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | a11ycheck ltr rtl | showcomments addcomment',
    autosave_ask_before_unload: true,
    autosave_interval: "30s",
    autosave_prefix: "{path}{query}-{id}-",
    autosave_restore_when_empty: false,
    autosave_retention: "2m",
    image_advtab: true,
    content_css: [
        '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
        '//www.tiny.cloud/css/codepen.min.css'
    ],
    link_list: [
        { title: 'Sitio oficial Boostic', value: 'https://www.boostic.com.mx' },
        { title: 'TinyMCE', value: 'https://www.tiny.cloud' },
        { title: 'Datatables', value: 'https://datatables.net' }
    ],
    image_list: [
        { title: 'Imagen 1', value: 'https://www.boostic.com.mx' },
        { title: 'Imagen 2', value: 'https://www.tiny.cloud' },
        { title: 'Imagen 3', value: 'https://datatables.net' }
    ],
    image_class_list: [
        { title: 'Ninguna', value: '' },
        { title: 'Clase 1', value: 'class-name' }
    ],
    importcss_append: true,
    height: 400,
    templates: [
        { title: 'Nueva tabla', description: 'Crea una tabla', content: '<div class="mceTmpl"><table width="98%%" border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
        { title: 'Nueva historia', description: 'Crea una historia', content: 'Había una vez...' },
        { title: 'Nueva lista con fechas', description: 'Crea una lista con fechas', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
    ],
    template_cdate_format: '[Fecha de creación (CDATE): %d/%m/%Y : %H:%M:%S]',
    template_mdate_format: '[Fecha de modificación (MDATE): %d/%m/%Y : %H:%M:%S]',
    image_caption: true,
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
    noneditable_noneditable_class: "mceNonEditable",
    toolbar_drawer: 'sliding',
    spellchecker_dialog: true,
    spellchecker_whitelist: ['Ephox', 'Moxiecode'],
    tinycomments_mode: 'embedded',
    content_style: ".mymention{color: gray; }",
    contextmenu: "link image imagetools table",
    language: 'es_MX'
});