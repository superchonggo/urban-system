<xsl:stylesheet version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:fn="http://www.wolterskluwer.com/functions"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	xpath-default-namespace="http://www.wolterskluwer.com/ssr/asset"
	exclude-result-prefixes="fn xs">


 <xsl:output method="text" />
 <xsl:variable name="xsltVersion" select="'04.02-0010-20180206'"/>

  <xsl:template match="/">


		<!--
	  <xsl:value-of select="//asset[@ofType]"/>
		-->
		
		<xsl:variable name='assetProduct'>
			<xsl:choose>
				<xsl:when test="/asset/assetInformation/productId">
					<xsl:value-of select="/asset/assetInformation/productId"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'medline'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:choose>
			<xsl:when test="$assetProduct='medline'">
				 <xsl:apply-templates/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="/asset[@ofType='article']">
						<xsl:apply-templates/>
					</xsl:when>
					<xsl:otherwise>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
  </xsl:template>


  <xsl:template match="* | node()">
    <xsl:apply-templates/>
  </xsl:template>


  <xsl:template match="asset">
	  <xsl:variable name='assetProduct'>
			<xsl:choose>
				<xsl:when test="assetInformation/productId">
					<xsl:value-of select="assetInformation/productId"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'medline'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		

	  <xsl:variable name='assetProductCat'>
			<xsl:choose>
				<xsl:when test="$assetProduct='medline'">
					<xsl:value-of select="'Medline'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Journal'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
			
			
		<!-- temp conditional handling, will be removed late  Stripped Version from wkmrid-->	
		<xsl:variable name='assetVersion'>	
			<xsl:choose>
				<xsl:when test="assetInformation/assetVersion and $assetProduct='medline'">
					<!--
					<xsl:value-of select="concat(concat(assetInformation/assetVersion,'.'),fn:generateAssetVersion(assetInformation/wkmrid))"/>
					-->
					<xsl:value-of select="concat(concat(concat(substring('0000', string-length(assetInformation/assetVersion) +1), assetInformation/assetVersion),'.'),fn:generateAssetVersion(assetInformation/wkmrid))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="fn:generateAssetVersion(assetInformation/wkmrid)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name='wkmrid'>	
			<xsl:choose>
				<xsl:when test="assetInformation/assetVersion and $assetProduct='medline'">
					<xsl:value-of select="concat(substring-before(assetInformation/wkmrid, '~'),'/root')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-before(assetInformation/wkmrid, '/v/')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name='fullwkmrid'>	
				<xsl:value-of select="assetInformation/wkmrid"/>
		</xsl:variable>
				
		<xsl:variable name="publisherStatus">
			<xsl:value-of select="//asset/assetInformation/assetVersion/@ofType"/>
		</xsl:variable>
		
		<xsl:variable name="publisherVersion">
			<xsl:value-of select="//asset/assetInformation/assetVersion/valueList/value[@valueType='source']/plainText[1]"/>
		</xsl:variable>
			
		<xsl:variable name='accessionNumber'>	
			 <xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/externalIdentifierList/externalIdentifier[@ofType='accession-number']/valueList[1]/value[1]"/>
		</xsl:variable>
		
		<xsl:variable name='sourceId'>	
			 <xsl:value-of select="//asset/metadataList/metadata[(@ofType='record' or @ofType='database-record')]/externalIdentifierList/externalIdentifier[@ofType='pmid']/valueList[1]/value[1]"/>
		</xsl:variable>
		
		<xsl:variable name='doi'>	
			 <xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/externalIdentifierList/externalIdentifier[@ofType='doi'][1]/valueList[1]"/>
		</xsl:variable>

	  <xsl:variable name='issn'>
				<xsl:value-of select="//asset/metadataList/metadata[@ofType='journal']/externalIdentifierList/externalIdentifier[@ofType='e-issn' or @ofType='p-issn' or @ofType='issn-l']/valueList/value"/>
		</xsl:variable>
		
		<xsl:variable name='issue'>
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='issue']/taxonomyIdentifierList/taxonomyIdentifier[@ofType='issue-number']/valueList/value"/>
		</xsl:variable>
		
	  <xsl:variable name='volume'>
			<xsl:choose>
				<xsl:when test="$assetProduct='medline'">
					<xsl:value-of select="//asset/metadataList/metadata[@ofType='volume']/taxonomyIdentifierList/taxonomyIdentifier[@ofType='volume']/valueList/value"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="//asset/metadataList/metadata[@ofType='issue']/taxonomyIdentifierList/taxonomyIdentifier[@ofType='volume']/valueList/value"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
	
	  <xsl:variable name='productVersion'>
			<xsl:value-of select="//asset/assetInformation/productVersion"/>
		</xsl:variable>
	
	  <xsl:variable name='title'>
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/titleList/title[@ofType='title']"/>
		</xsl:variable>
		
		<xsl:variable name='subTitle'>
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/titleList/title[@ofType='sub-title']"/>
		</xsl:variable>
		
		<xsl:variable name='pageRange_firstPage'>
			<!--
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/pagination/textFormatList/textFormat[@ofType='input-data']/plainText"/>
			-->
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/pagination/pageRangeList/pageRange/firstPage/valueList/value/plainText"/>
		</xsl:variable>
		
		<xsl:variable name="authors">
      <xsl:for-each select="//asset/metadataList/metadata[@ofType='article']/contributorList/contributor[@ofType='author']">
        <xsl:variable name="pos">
          <xsl:number/>
        </xsl:variable>
        <xsl:variable name="lastName" select="normalize-space(nameList/name/lastName)"/>
        <xsl:variable name="firstName" select="normalize-space(nameList/name/firstName)"/>
         
        <xsl:choose>
          <xsl:when test="$pos &gt; 1">
            <xsl:value-of select="concat(';', string-join(($lastName, $firstName)[.!=''], ','))"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="string-join(($lastName, $firstName)[.!=''], ',')"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </xsl:variable>
		
		<xsl:variable name='publicationSort'>
			 <xsl:choose>
          <xsl:when test="$assetProduct='medline'">
						<xsl:value-of select="//asset/metadataList/metadata[@ofType='issue' or @ofType='book']/publicationHistoryList/publicationHistory/publicationDate/sortedValue"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/publicationHistoryList/publicationHistory/publicationDate/sortedValue"/>
          </xsl:otherwise>
        </xsl:choose>
			<!--
			<xsl:value-of select="//asset/metadataList/metadata[@ofType='article']/publicationHistoryList/publicationHistory/publicationDate/sortedValue"/>
			-->
		</xsl:variable>
			<xsl:variable name='isPAP'>
			<xsl:value-of select="//asset/assetInformation/flagList/flag[@ofType='pap']/@value"/>
		</xsl:variable>
		
		<xsl:value-of select="$assetProduct"/>|<xsl:value-of select="$assetProductCat"/>|Document|<xsl:value-of select="$fullwkmrid"/>|<xsl:value-of select="$wkmrid"/>|<xsl:value-of select="$assetVersion"/>|<xsl:value-of select="normalize-space($accessionNumber)"/>|<xsl:value-of select="normalize-space($sourceId)"/>|<xsl:value-of select="replace(normalize-space($doi),'\|','-')"/>|<xsl:value-of select="replace(normalize-space($issn),' ',',')"/>|<xsl:value-of select="replace(normalize-space($issue),'\|','-')"/>|<xsl:value-of select="replace(normalize-space($volume),'\|','-')"/>|<xsl:value-of select="$productVersion"/>|<xsl:value-of select="@status"/>|<xsl:value-of select="replace($pageRange_firstPage,'\|','-')"/>|<xsl:value-of select="replace(normalize-space($title),'\|','-')"/>|<xsl:value-of select="replace(normalize-space($subTitle),'\|','-')"/>|<xsl:value-of select="$authors"/>|<xsl:value-of select="$publicationSort"/>|<xsl:value-of select="$isPAP"/>|<xsl:value-of select="$publisherStatus"/>|<xsl:value-of select="$publisherVersion"/>
		<!--
		<xsl:text>&#xa;</xsl:text>
		-->
  </xsl:template>

  	
	<xsl:function name="fn:generateAssetVersion">
    <xsl:param name="wkmridValue"/>
    <xsl:variable name="afterV" select="substring-after($wkmridValue, '/v/')"/>
    <xsl:variable name="result">
      <xsl:choose>
        <xsl:when test="contains($afterV, '/r/')">
          <xsl:value-of select="substring-before($afterV,'/r/')"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$afterV"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:sequence select="$result"/>
  </xsl:function>
	
	<xsl:function name="fn:stripAssetVersion">
	
	
	</xsl:function>
		
	<!--
	<xsl:template name="createField">
    <xsl:param name="name"/>
    <xsl:param name="value"/>
    <xsl:element name="field">
      <xsl:attribute name="name" select="$name"/>
      <xsl:value-of select="normalize-space($value)"/>
    </xsl:element>
  </xsl:template>
	-->
	
</xsl:stylesheet>
